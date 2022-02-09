using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TINY_Compiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }
        Node Program()
        {
            Node program = new Node("Program");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.Comment)
                {
                    program.Children.Add(match(Token_Class.Comment));
                }
                if (TokenStream[InputPointer].token_type == Token_Class.String ||
                   TokenStream[InputPointer].token_type == Token_Class.Float)
                {
                    program.Children.Add(FunctionStatements());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Int)
                {
                    if (TokenStream[InputPointer + 1].token_type == Token_Class.Idenifier)
                        program.Children.Add(FunctionStatements());
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected Datatype"
                          + "\r\n");
                    InputPointer++;

                }
                program.Children.Add(MainBlock());
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected Main body"
                          + "\r\n");
            }
            MessageBox.Show("Success");
            return program;
        }


        Node FunctionStatements()
        {
            Node functionStatement = new Node("FunctionStatements");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.Int ||
                TokenStream[InputPointer].token_type == Token_Class.String ||
               TokenStream[InputPointer].token_type == Token_Class.Float)
                {
                    functionStatement.Children.Add(FunctionDeclaration());
                    functionStatement.Children.Add(FunctionBody());
                    if (TokenStream[InputPointer + 1].token_type == Token_Class.Main)
                        return functionStatement;
                    else
                        functionStatement.Children.Add(FunctionStatements());
                    return functionStatement;
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Wrong Datatype"
                          + "\r\n");
                    InputPointer++;
                    return null;
                }
            }
            // write your code here to check the header sructure
            return null;
        }

       

        Node FunctionDeclaration()
        {
            Node functionDeclaration = new Node("FunctionDeclaration");
            if (InputPointer < TokenStream.Count)
            {


                // write your code here to check atleast the declare sturcure
                if (TokenStream[InputPointer].token_type == Token_Class.Int)
                    functionDeclaration.Children.Add(match(Token_Class.Int));
                else if (TokenStream[InputPointer].token_type == Token_Class.Float)
                    functionDeclaration.Children.Add(match(Token_Class.Float));
                else if (TokenStream[InputPointer].token_type == Token_Class.String)
                    functionDeclaration.Children.Add(match(Token_Class.String));
                else if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
                {

                    Errors.Error_List.Add("Parsing Error: Wrong Datatype"
                          + "\r\n");
                    InputPointer++;
                    return null;
                }
                functionDeclaration.Children.Add(match(Token_Class.Idenifier));
                functionDeclaration.Children.Add(match(Token_Class.LParanthesis));
                if (TokenStream[InputPointer].token_type == Token_Class.Int ||
                     TokenStream[InputPointer].token_type == Token_Class.String ||
                    TokenStream[InputPointer].token_type == Token_Class.Float)
                {
                    functionDeclaration.Children.Add(ArgumentList());

                }
                else if (TokenStream[InputPointer].token_type != Token_Class.RParanthesis)
                {
                    Errors.Error_List.Add("Parsing Error: Wrong Datatype"
                          + "\r\n");
                    InputPointer++;
                    return null;
                }
                functionDeclaration.Children.Add(match(Token_Class.RParanthesis));
            }
            return functionDeclaration;

            // without adding procedures
        }
        Node ArgumentList()
        {
            Node argumentList = new Node("ArgumentList");
            if (InputPointer < TokenStream.Count)
            {
                // write your code here to match statements
                if (TokenStream[InputPointer].token_type == Token_Class.Int)
                    argumentList.Children.Add(match(Token_Class.Int));
                else if (TokenStream[InputPointer].token_type == Token_Class.Float)
                    argumentList.Children.Add(match(Token_Class.Float));
                else if (TokenStream[InputPointer].token_type == Token_Class.String)
                    argumentList.Children.Add(match(Token_Class.String));
                else
                {
                    Errors.Error_List.Add("Parsing Error: Wrong Datatype"
                          + "\r\n");
                    InputPointer++;
                    return null;
                }
                argumentList.Children.Add(match(Token_Class.Idenifier));
                argumentList.Children.Add(Argument());
            }
            return argumentList;
        }
        Node Argument()
        {
            Node argument = new Node("Argument");
            if (InputPointer < TokenStream.Count)
            {
                // write your code here to match statements
                if (TokenStream[InputPointer].token_type != Token_Class.RParanthesis)
                {
                    if (TokenStream[InputPointer].token_type == Token_Class.Comma)
                    {
                        argument.Children.Add(match(Token_Class.Comma));
                        argument.Children.Add(ArgumentList());
                        return argument;
                    }
                    else
                    {
                        Errors.Error_List.Add("Parsing Error: Expected Comma "
                                             + "\r\n");
                        InputPointer++;
                        return null;
                    }
                }
            }
            return null;
        }
        Node Statements()
        {
            Node statements = new Node("Statements");
            if (InputPointer < TokenStream.Count)
            {
                // write your code here to match statements
                if (TokenStream[InputPointer].token_type == Token_Class.Repeat ||
                TokenStream[InputPointer].token_type == Token_Class.Idenifier ||
                TokenStream[InputPointer].token_type == Token_Class.Int ||
                 TokenStream[InputPointer].token_type == Token_Class.String ||
                TokenStream[InputPointer].token_type == Token_Class.Float ||
                TokenStream[InputPointer].token_type == Token_Class.Write ||
                 TokenStream[InputPointer].token_type == Token_Class.Read ||
                TokenStream[InputPointer].token_type == Token_Class.If)
                {
                    statements.Children.Add(Statement());
                    statements.Children.Add(Statements());
                    return statements;
                }
            }

            return null;
        }
       
        Node Statement()
        {
            Node statement = new Node("Statement");
            if (InputPointer < TokenStream.Count)
            {
                // write your code here to match statements          
                if (TokenStream[InputPointer].token_type == Token_Class.Repeat)
                    statement.Children.Add(ReapetStatement());
                else if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
                {
                    statement.Children.Add(match(Token_Class.Idenifier));
                    statement.Children.Add(State());

                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Int ||
                    TokenStream[InputPointer].token_type == Token_Class.Float ||
                    TokenStream[InputPointer].token_type == Token_Class.String)
                {
                    statement.Children.Add(DeclarationStatement());
                    statement.Children.Add(match(Token_Class.Semicolon));

                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Write)
                {
                    statement.Children.Add(WriteStatement());
                    statement.Children.Add(match(Token_Class.Semicolon));

                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Read)
                {
                    statement.Children.Add(ReadStatement());
                    statement.Children.Add(match(Token_Class.Semicolon));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.If)
                    statement.Children.Add(IfStatement());
                else if (TokenStream[InputPointer].token_type == Token_Class.Return)
                {
                    statement.Children.Add(ReturnStatement());
                }
            }
            return statement;

        }
        Node State()
        {
            Node state = new Node("state");
            if (InputPointer < TokenStream.Count)
            {
                // write your code here to match statements
                if (TokenStream[InputPointer].token_type == Token_Class.AssignEqual)
                {
                    state.Children.Add(AssignStatement());
                    state.Children.Add(match(Token_Class.Semicolon));

                }
                else if (TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
                {

                    state.Children.Add(Ter());

                }


            }
            return state;
        }
        // Implement your logic here
        Node FunctionBody()
        {
            Node functionBody = new Node("functionBody");
            if (InputPointer < TokenStream.Count)
            {
                // write your code here to match statements        
                functionBody.Children.Add(match(Token_Class.LCurlyBracket));
                functionBody.Children.Add(Statements());
                functionBody.Children.Add(ReturnStatement());
                functionBody.Children.Add(match(Token_Class.RCurlyBracket));
            }
            return functionBody;
        }
        Node ReturnStatement()
        {
            Node returnStatement = new Node("returnStatement");
            if (InputPointer < TokenStream.Count)
            {
                // write your code here to match statements        
                returnStatement.Children.Add(match(Token_Class.Return));
                if (TokenStream[InputPointer].token_type == Token_Class.StringStatement ||
                    TokenStream[InputPointer].token_type == Token_Class.Idenifier ||
                    TokenStream[InputPointer].token_type == Token_Class.Constant ||
                    TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
                {
                    returnStatement.Children.Add(Expression());

                }
                returnStatement.Children.Add(match(Token_Class.Semicolon));
            }
            return returnStatement;

        }
        Node Expression()
        {
            Node expression = new Node("expression");
            if (InputPointer < TokenStream.Count)
            {
                // write your code here to match statements        
                if (TokenStream[InputPointer].token_type == Token_Class.StringStatement)
                {
                    expression.Children.Add(match(Token_Class.StringStatement));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Idenifier ||
                   TokenStream[InputPointer].token_type == Token_Class.Constant)
                {

                    expression.Children.Add(Term());
                    expression.Children.Add(Exp());

                }
                else if (TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
                {
                    expression.Children.Add(match(Token_Class.LParanthesis));
                    expression.Children.Add(Equation());
                    expression.Children.Add(match(Token_Class.RParanthesis));
                    expression.Children.Add(Eq());

                }
            }

            return expression;
        }
        Node Exp()
        {
            Node exp = new Node("Exp");
            // write your code here to match statements        
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.PlusOp ||
                TokenStream[InputPointer].token_type == Token_Class.MinusOp ||
                TokenStream[InputPointer].token_type == Token_Class.DivideOp ||
                TokenStream[InputPointer].token_type == Token_Class.MultiplyOp)
                {
                    exp.Children.Add(Eq());
                    return exp;
                }
            }
            return null;
        }
        Node Term()
        {
            Node term = new Node("term");
            // write your code here to match statements 
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.Constant)
                {
                    term.Children.Add(match(Token_Class.Constant));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
                {
                    term.Children.Add(match(Token_Class.Idenifier));
                    term.Children.Add(Ter());

                }
            }
            return term;
        }
        Node Ter()
        {
            Node ter = new Node("FunctionCall");
            // write your code here to match statements 
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
                {
                    ter.Children.Add(match(Token_Class.LParanthesis));
                    if (TokenStream[InputPointer].token_type == Token_Class.Idenifier ||
                        TokenStream[InputPointer].token_type == Token_Class.Constant)
                    {
                        ter.Children.Add(Parameters());
                    }
                    ter.Children.Add(match(Token_Class.RParanthesis));
                    return ter;

                }

            }
            return null;
        }

        Node Parameters()
        {
            Node parameters = new Node("parameters");
            if (InputPointer < TokenStream.Count)
            {
                // write your code here to match statements
                if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
                {
                    parameters.Children.Add(match(Token_Class.Idenifier));

                }
                else
                    parameters.Children.Add(match(Token_Class.Constant));
                parameters.Children.Add(Parameter());
            }
            return parameters;
        }

        Node Parameter()
        {
            Node parameter = new Node("parameter");
            if (InputPointer < TokenStream.Count)
            {
                // write your code here to match statements        
                if (TokenStream[InputPointer].token_type == Token_Class.Comma)
                {
                    parameter.Children.Add(match(Token_Class.Comma));
                    parameter.Children.Add(Parameters());
                    return parameter;
                }
            }
            return null;
        }


        Node Equation()
        {
            Node equation = new Node("equation");
            if (InputPointer < TokenStream.Count)
            {
                // write your code here to match statements
                if (TokenStream[InputPointer].token_type == Token_Class.Constant ||
                    TokenStream[InputPointer].token_type == Token_Class.Idenifier)
                {
                    equation.Children.Add(Term());
                    equation.Children.Add(Eq());

                }
                else if (TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
                {
                    equation.Children.Add(match(Token_Class.LParanthesis));
                    equation.Children.Add(Equation());
                    equation.Children.Add(match(Token_Class.RParanthesis));
                    equation.Children.Add(Eq());
                }
            }
            return equation;
        }

        Node Eq()
        {
            Node eq = new Node("eq");
            if (InputPointer < TokenStream.Count)
            {
                // write your code here to match statements        
                if (TokenStream[InputPointer].token_type == Token_Class.PlusOp ||
                TokenStream[InputPointer].token_type == Token_Class.MinusOp ||
                TokenStream[InputPointer].token_type == Token_Class.DivideOp ||
                TokenStream[InputPointer].token_type == Token_Class.MultiplyOp)
                {
                    eq.Children.Add(ArithmeticOp());
                    eq.Children.Add(Equation());
                    return eq;
                }
            }
            return null;
        }
        Node ArithmeticOp()
        {
            Node arithmeticOp = new Node("arithmeticOp");
            if (InputPointer < TokenStream.Count)
            {
                // write your code here to match statements 
                if (TokenStream[InputPointer].token_type == Token_Class.MinusOp)
                {
                    arithmeticOp.Children.Add(match(Token_Class.MinusOp));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.MultiplyOp)
                {
                    arithmeticOp.Children.Add(match(Token_Class.MultiplyOp));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.DivideOp)
                {
                    arithmeticOp.Children.Add(match(Token_Class.DivideOp));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.PlusOp)
                {
                    arithmeticOp.Children.Add(match(Token_Class.PlusOp));
                }
            }
            return arithmeticOp;
        }
      

        Node ReapetStatement()
        {
            Node reapetstatement = new Node("ReapetStatement");
            if (InputPointer < TokenStream.Count)
            {
                reapetstatement.Children.Add(match(Token_Class.Repeat));
                reapetstatement.Children.Add(Statements());
                reapetstatement.Children.Add(match(Token_Class.Until));
                reapetstatement.Children.Add(ConditionStatement());
            }
            return reapetstatement;
        }

        Node ConditionStatement()
        {
            Node conditionstatement = new Node("ConditionStatement");
            if (InputPointer < TokenStream.Count)
            {
                conditionstatement.Children.Add(Condition());
                conditionstatement.Children.Add(MultiCondition());
            }
            return conditionstatement;
        }

        Node Condition()
        {
            Node condition = new Node("Condition");
            if (InputPointer < TokenStream.Count)
            {
                condition.Children.Add(match(Token_Class.Idenifier));
                condition.Children.Add(ConditionOP());
                condition.Children.Add(Term());
            }
            return condition;
        }

        Node ConditionOP()
        {
            Node conditionop = new Node("ConditionOP");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.LessThanOp)
                {
                    conditionop.Children.Add(match(Token_Class.LessThanOp));

                }
                else if (TokenStream[InputPointer].token_type == Token_Class.GreaterThanOp)
                {
                    conditionop.Children.Add(match(Token_Class.GreaterThanOp));

                }
                else if (TokenStream[InputPointer].token_type == Token_Class.EqualOp)
                {
                    conditionop.Children.Add(match(Token_Class.EqualOp));

                }

                else if (TokenStream[InputPointer].token_type == Token_Class.NotEqualOp)
                {
                    conditionop.Children.Add(match(Token_Class.NotEqualOp));

                }
            }
            return conditionop;
        }

        Node MultiCondition()
        {
            Node multicondition = new Node("MultiCondition");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.AndOp ||
                TokenStream[InputPointer].token_type == Token_Class.OrOp)
                {
                    multicondition.Children.Add(BoolOP());
                    multicondition.Children.Add(Condition());
                    multicondition.Children.Add(MultiCondition());
                    return multicondition;
                }
            }
            return null;

        }

        Node BoolOP()
        {
            Node boolop = new Node("BoolOP");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.AndOp)
                {
                    boolop.Children.Add(match(Token_Class.AndOp));

                }
                else if (TokenStream[InputPointer].token_type == Token_Class.OrOp)
                {
                    boolop.Children.Add(match(Token_Class.OrOp));

                }


            }
            return boolop;
        }

        Node AssignStatement()
        {
            Node assignstatement = new Node("AssignStatement");
            if (InputPointer < TokenStream.Count)
            {
                assignstatement.Children.Add(match(Token_Class.AssignEqual));
                assignstatement.Children.Add(Expression());
            }
            return assignstatement;
        }

        Node DeclarationStatement()
        {
            Node declarationStatement = new Node("DeclarationStatement");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.Int)
                {
                    declarationStatement.Children.Add(match(Token_Class.Int));

                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Float)
                {
                    declarationStatement.Children.Add(match(Token_Class.Float));

                }
                else if (TokenStream[InputPointer].token_type == Token_Class.String)
                {
                    declarationStatement.Children.Add(match(Token_Class.String));

                }
                declarationStatement.Children.Add(match(Token_Class.Idenifier));
                declarationStatement.Children.Add(Declare());

            }

            return declarationStatement;
        }

        Node Declare()
        {
            Node declare = new Node(" Declare");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.AssignEqual)
                {
                    declare.Children.Add(AssignStatement());
                    declare.Children.Add(MultiDeclaration());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Comma)
                {
                    declare.Children.Add(MultiDeclaration());

                }
                return declare;

            }
            return null;
        }

        Node MultiDeclaration()
        {
            Node multideclaration = new Node("MultiDeclaration");
            if (InputPointer < TokenStream.Count)
            {

                if (TokenStream[InputPointer].token_type == Token_Class.Comma)
                {
                    multideclaration.Children.Add(match(Token_Class.Comma));
                    multideclaration.Children.Add(match(Token_Class.Idenifier));
                    multideclaration.Children.Add(Multi());
                    multideclaration.Children.Add(MultiDeclaration());
                    return multideclaration;
                }

            }

            return null;

        }


        Node Multi()
        {
            Node multi = new Node(" Multi");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.AssignEqual)
                {
                    multi.Children.Add(AssignStatement());
                    return multi;
                }

            }
            return null;
        }
        
        


        Node WriteStatement()
        {
            Node writestatement = new Node("WriteStatement");
            if (InputPointer < TokenStream.Count)
            {
                writestatement.Children.Add(match(Token_Class.Write));

                if (TokenStream[InputPointer].token_type == Token_Class.Endl)
                {
                    writestatement.Children.Add(match(Token_Class.Endl));
                }
                
                else
                    writestatement.Children.Add(Expression());
            }

            return writestatement;
        }

        Node ReadStatement()
        {
            Node readStatement = new Node("ReadStatement");
            if (InputPointer < TokenStream.Count)
            {
                readStatement.Children.Add(match(Token_Class.Read));
                readStatement.Children.Add(match(Token_Class.Idenifier));
            }
            return readStatement;
        }

        Node IfStatement()
        {
            Node ifStatement = new Node("IfStatement");
            if (InputPointer < TokenStream.Count)
            {
                ifStatement.Children.Add(match(Token_Class.If));
                ifStatement.Children.Add(ConditionStatement());
                ifStatement.Children.Add(match(Token_Class.Then));
                ifStatement.Children.Add(IFStatements());

                if (TokenStream[InputPointer].token_type == Token_Class.Elseif)
                {
                    ifStatement.Children.Add(ElseifStatement());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Else)
                {
                    ifStatement.Children.Add(ElseStatement());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.End)
                {
                    ifStatement.Children.Add(match(Token_Class.End));
                }
            }
            return ifStatement;
        }
        Node ElseifStatement()
        {
            Node elseifStatement = new Node("ElseIfStatement");
            if (InputPointer < TokenStream.Count)
            {
                elseifStatement.Children.Add(match(Token_Class.Elseif));
                elseifStatement.Children.Add(ConditionStatement());
                elseifStatement.Children.Add(match(Token_Class.Then));
                elseifStatement.Children.Add(IFStatements());

                if (TokenStream[InputPointer].token_type == Token_Class.Elseif)
                {
                    elseifStatement.Children.Add(ElseifStatement());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Else)
                {
                    elseifStatement.Children.Add(ElseStatement());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.End)
                {
                    elseifStatement.Children.Add(match(Token_Class.End));
                }
            }
            return elseifStatement;
        }

        Node ElseStatement()
        {
            Node elseStatement = new Node("ElseStatement");
            if (InputPointer < TokenStream.Count)
            {
                elseStatement.Children.Add(match(Token_Class.Else));
                elseStatement.Children.Add(Statements());
                elseStatement.Children.Add(match(Token_Class.End));
            }

            return elseStatement;
        }

        Node IFStatements()
        {
            Node iFStatements = new Node(" IFStatements");
            if (InputPointer < TokenStream.Count)
            {
                // write your code here to match statements
                if (TokenStream[InputPointer].token_type == Token_Class.Repeat ||
                TokenStream[InputPointer].token_type == Token_Class.Idenifier ||
                TokenStream[InputPointer].token_type == Token_Class.Int ||
                 TokenStream[InputPointer].token_type == Token_Class.String ||
                TokenStream[InputPointer].token_type == Token_Class.Float ||
                TokenStream[InputPointer].token_type == Token_Class.Write ||
                 TokenStream[InputPointer].token_type == Token_Class.Read ||
                TokenStream[InputPointer].token_type == Token_Class.If ||
                TokenStream[InputPointer].token_type == Token_Class.Return)
                {
                    iFStatements.Children.Add(Statement());
                    iFStatements.Children.Add(IFStatements());
                    return iFStatements;
                }
            }
            return null;
        }

        Node MainBlock()
        {
            Node mainBlock = new Node("MainBlock");
            if (InputPointer < TokenStream.Count)
            {
                mainBlock.Children.Add(match(Token_Class.Int));
                mainBlock.Children.Add(match(Token_Class.Main));
                mainBlock.Children.Add(match(Token_Class.LParanthesis));
                mainBlock.Children.Add(match(Token_Class.RParanthesis));
                mainBlock.Children.Add(FunctionBody());
            }

            return mainBlock;
        }


        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {

                if (InputPointer == 0 && TokenStream[InputPointer].token_type.Equals(Token_Class.Comment))
                {
                    InputPointer++;
                    return null;

                }

                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    if ((InputPointer + 1) < TokenStream.Count && TokenStream[InputPointer + 1].token_type.Equals(Token_Class.Comment))
                    {

                        InputPointer++;
                    }
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }

                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }


            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}

