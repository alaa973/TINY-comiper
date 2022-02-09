using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public enum Token_Class
{
    End, Repeat, Else, If, Int, Read, Then, Until, Write,
    Semicolon, Comma, LParanthesis, RParanthesis, EqualOp, LessThanOp,
    GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp,
    Idenifier, Constant, String, Float, Elseif, Return, Endl, Comment,
    AndOp, OrOp, LCurlyBracket, RCurlyBracket, Main, AssignEqual, StringStatement
}
namespace TINY_Compiler
{


    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();
        public List<string> errors = new List<string>();

        public Scanner()
        {

            ReservedWords.Add("main", Token_Class.Main);
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("elseif", Token_Class.Elseif);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("write", Token_Class.Write);

            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("{", Token_Class.LCurlyBracket);
            Operators.Add("}", Token_Class.RCurlyBracket);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add(":=", Token_Class.AssignEqual);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("&&", Token_Class.AndOp);
            Operators.Add("||", Token_Class.OrOp);




        }

        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {


                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();
                // //checks if it's an Empty String
                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;

                if (char.IsLetter(CurrentChar) || char.IsDigit(CurrentChar)) //checks if it's a Letter or Number
                {
                    j++;
                    /*if (j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                    }*/
                    while (j < SourceCode.Length && (char.IsLetterOrDigit(SourceCode[j]) || SourceCode[j] == '.'))
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                    }

                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }


                else if (CurrentChar == '{' || CurrentChar == '}' || CurrentChar == '(' || CurrentChar == ')'
                    || CurrentChar == '=' || CurrentChar == '>' || CurrentChar == ',' || CurrentChar == ';' ||
                    CurrentChar == '*' || CurrentChar == '+' || CurrentChar == '-')
                {
                    FindTokenClass(CurrentLexeme);
                }
                else if (CurrentChar == '<')
                {
                    if (j + 1 < SourceCode.Length)
                    {
                        j++;
                        CurrentChar = SourceCode[j];
                        if (CurrentChar == '>')
                        {
                            CurrentLexeme += CurrentChar;
                            i = j;
                        }
                        else
                        {
                            j--;
                            CurrentChar = SourceCode[j];
                            i = j;
                        }

                    }
                    FindTokenClass(CurrentLexeme);
                }
                //checks if it's an Assignment Statement
                else if (CurrentChar == ':')
                {
                    if (j + 1 < SourceCode.Length)
                    {
                        j++;
                        CurrentChar = SourceCode[j];
                        if (CurrentChar == '=')
                        {
                            CurrentLexeme += CurrentChar;
                            i = j;
                        }
                        else
                        {
                            j--;
                            CurrentChar = SourceCode[j];
                            i = j;
                        }
                    }
                    FindTokenClass(CurrentLexeme);
                }
                //checks if it's an AND Statement
                else if (CurrentChar == '&')
                {
                    if (j + 1 < SourceCode.Length)
                    {
                        j++;
                        CurrentChar = SourceCode[j];
                        if (CurrentChar == '&')
                        {
                            CurrentLexeme += CurrentChar;
                            i = j;
                        }   
                        else
                        {
                            j--;
                            CurrentChar = SourceCode[j];
                            i = j;
                        }
                    }
                    FindTokenClass(CurrentLexeme);
                }
                //checks if it's an OR Statement
                else if (CurrentChar == '|')
                {
                    if (j + 1 < SourceCode.Length)
                    {
                        j++;
                        CurrentChar = SourceCode[j];
                        if (CurrentChar == '|')
                        {
                            CurrentLexeme += CurrentChar;
                            i = j;
                        }
                        else
                        {
                            j--;
                            CurrentChar = SourceCode[j];
                            i = j;
                        }
                    }
                    FindTokenClass(CurrentLexeme);
                }
                //checks if it's an Comment Statement
                else if (CurrentChar == '/')
                {
                    if (j + 1 < SourceCode.Length)
                    {

                        j++;
                        CurrentChar = SourceCode[j];
                        if (CurrentChar == '*')
                        {
                            while (j < SourceCode.Length)
                            {

                                CurrentChar = SourceCode[j];
                                CurrentLexeme += CurrentChar;
                                if (j + 1 < SourceCode.Length && CurrentChar == '*' && SourceCode[j + 1] == '/')
                                {

                                    CurrentLexeme += SourceCode[j + 1];
                                    break;

                                }

                                j++;
                            }

                            i = j + 1;

                        }
                        else
                        {
                            j--;
                            CurrentChar = SourceCode[j];
                            i = j;
                        }
                    }

                    FindTokenClass(CurrentLexeme);
                }
                //checks if it's an String Statement
                else if (CurrentChar == '"')
                {
                    if (j + 1 < SourceCode.Length)
                    {

                        j++;
                        CurrentChar = SourceCode[j];

                        while (j < SourceCode.Length)
                        {

                            CurrentChar = SourceCode[j];
                            CurrentLexeme += CurrentChar;
                            if (CurrentChar == '"')
                            {

                                break;

                            }

                            j++;
                        }

                        i = j;



                    }
                    FindTokenClass(CurrentLexeme);
                }
                else
                {
                    // checks if it's an empty string
                    if (CurrentChar == '\t' || CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                        continue;
                    FindTokenClass(CurrentLexeme);
                }


            }

            TINY_Compiler.TokenStream = Tokens;
            Errors.Error_List = errors;
        }
        void FindTokenClass(string Lex)
        {

            Token Tok = new Token();
            Tok.lex = Lex;
            Console.WriteLine(Lex);

            //Is it a reserved word?
            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }
            //Is it an operator?  
            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }
            //Is it an identifier? alaa adel

            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Idenifier;
                Tokens.Add(Tok);

            }
            //Is it a Constant?  asmaa ashraf
            else if (isConstant(Lex))
            {
                Tok.token_type = Token_Class.Constant;
                Tokens.Add(Tok);

            }



            //Is it a comment?  alaa mostafa
            else if (isComment(Lex))
            {
                Tok.token_type = Token_Class.Comment;
                Tokens.Add(Tok);
            }

            //Is it a StringStatement? alaa amr
            else if (isStringStatement(Lex))
            {
                Tok.token_type = Token_Class.StringStatement;
                Tokens.Add(Tok);
            }

            //Is it an undefined?   eslam esam
            else
            {
                errors.Add(Lex);
            }
        }



        bool isIdentifier(string lex)
        {
            bool isValid = true;

            // Check if the lex is an identifier or not.
            var regIdentifier = new Regex("^[a-zA-Z]([a-zA-Z0-9])*$", RegexOptions.Compiled);


            if (!(regIdentifier.IsMatch(lex)))
            {
                isValid = false;
            }


            return isValid;
        }
        bool isConstant(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.
            var regConstant = new Regex("^[0-9]+([.][0-9]+)?$", RegexOptions.Compiled);
            if (!(regConstant.IsMatch(lex)))
            {
                isValid = false;
            }
            return isValid;
        }
        bool isComment(string lex)
        {
            bool isValid = true;
            // Check if the lex is a comment or not.
            /* */
            //var regComment = new Regex(@"^/\*(.||\s)*\*/$", RegexOptions.Compiled);
            var regComment = new Regex(@"(?s)/\\*.*?\\*/", RegexOptions.Compiled);
            if (!(regComment.IsMatch(lex)))
            {
                isValid = false;
            }
            return isValid;
        }
        bool isStringStatement(string lex)
        {
            bool isValid = true;
            // Check if the lex is a string statement or not.
            /* */
            var regStringStatement = new Regex("^\"(.*)\"$", RegexOptions.Compiled);
            if (!(regStringStatement.IsMatch(lex)))
            {
                isValid = false;
            }
            return isValid;
        }
    }
}
