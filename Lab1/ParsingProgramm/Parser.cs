using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB
{
    struct Statement
    {
        public string value;
        public int position;

        public Statement(string val, int pos)
        {
            value = val;
            position = pos;
        }
    }

    enum IfStatus : byte
    {
        Unknown,
        Single,
        Couples
    }

    class IfStatement
    {
        public IfStatus Status { get; set; }
        public int LeftCurlyBraces { get; set; }
        public int RightCurlyBraces { get; set; }

        public IfStatement()
        {
            Status = IfStatus.Unknown;
            LeftCurlyBraces = RightCurlyBraces = 0;
        }

        public int SingleBraces => 
            LeftCurlyBraces - RightCurlyBraces;
    }

    class Parser
    {
        public Parser(string path)
        {
            text = File.ReadAllText(path);

            initialCapacity = 100;
            statements = new Statement[initialCapacity];
            count = 0;

            ifStatements = new LinkedList<LinkedList<IfStatement>>();

            regex = new Regex(pattern);
        }


        public string[] GetArray() =>
            ConvertStatementsToStringArray();

        public void PrintStatements()
        {
            Console.WriteLine();
            for (int i = 0; i < count; i++)
                Console.WriteLine(i + " " + statements[i].value + "\t" + statements[i].position);
            Console.WriteLine();
        }

        public void Parse()
        {
            ifStatements.AddLast(new LinkedList<IfStatement>());
            Match match = regex.Match(text);

            while (match.Success)
            {
                switch (match.Value)
                {
                    case "if":
                        HandleIfCase(match.Index);
                        break;
                    case "else":
                        HandleElseCase(match.Index);
                        break;
                    case "{":
                        AddLeftCurlyBrace(match.Index);
                        break;
                    case "}":
                        AddRightCurlyBrace(match.Index);
                        break;
                }
                match = match.NextMatch();
            }

            RemoveLastBlockStatement();
        }

        
        private string text;
        private string pattern = @"(\bif\b)|(\belse\b)|({)|(})";
        private Regex regex;

        
        private int insertionCode = -1;
        private int count;
        private int initialCapacity;
        private Statement[] statements;

        private string[] ConvertStatementsToStringArray()
        {
            string[] array = new string[count + 1];

            for (int i = 0; i <= count; i++)
                array[i] = statements[i].value;

            array[count] = null;

            return array;
        }

        private int LastStatementPosition =>
            count == 0 ? insertionCode : statements[count - 1].position;

        private int AfterLastStatementPosition =>
            LastStatementPosition == insertionCode ?
                insertionCode : LastStatementPosition + LastStatementValue.Length;

        private string LastStatementValue =>
            count == 0 ? insertionCode.ToString() : statements[count - 1].value;

        private int Capacity => statements.Length;   


        private void Resize() => Array.Resize(ref statements, 2 * Capacity);

        private int AddStatement(string statement, int position)
        {
            if (count == Capacity)
                Resize();

            statements[count] = new Statement(statement, position);
            count++;

            return count - 1;
        }

        private void HandleIfCase(int position)
        {
            switch (LastStatementValue)
            {
                case "if":
                    HandleIfIf(position);
                    break;
                case "else":
                    HandleElseIf(position);
                    break;
                case "}":
                    ClearLastBlockStatement();
                    break;
            }

            ifStatements.Last.Value.AddLast(new IfStatement());
            AddStatement("if", position);
        }

        private void HandleElseCase(int position)
        {
            if (LastStatementValue == "else" || LastStatementValue == "}")
                InsertRightCurlyBraces();

            AddStatement("else", position);
            LastIfStatement.Status = IfStatus.Couples;
        }

        private void AddLeftCurlyBrace(int position)
        {
            ifStatements.AddLast(new LinkedList<IfStatement>());
            AddStatement("{", position);
        }

        private void AddRightCurlyBrace(int position)
        {
            RemoveLastBlockStatement();
            AddStatement("}", position);
        }


        private LinkedList<LinkedList<IfStatement>> ifStatements;

        private IfStatement LastIfStatement =>
            LastIfStatementNode?.Value;

        private LinkedListNode<IfStatement> LastIfStatementNode =>
            ifStatements?.Last?.Value?.Last;

        private LinkedListNode<IfStatement> FirstIfStatementNodeInLastList =>
            ifStatements?.Last?.Value?.First;

        private int FirstIfStatementNodeInLastListSingleBraces =>
            FirstIfStatementNodeInLastList == null ?
                0 : FirstIfStatementNodeInLastList.Value.SingleBraces;


        private LinkedListNode<IfStatement> FindLastIfWithUnknownStatus()
        {
            LinkedListNode<IfStatement> node = LastIfStatementNode;
            while (node != null && node.Value.Status != IfStatus.Unknown)
                node = node.Previous;

            return node;
        }

        private void TryRemoveLastIfStatement()
        {
            LinkedListNode<IfStatement> node = LastIfStatementNode;
            while (node != null && node.Value.Status != IfStatus.Unknown &&
                   node.Value.SingleBraces == 0)
            {
                node = node.Previous;
                ifStatements.Last.Value.RemoveLast();
            }
        }

        private void OnLeftCurlyBraceAdded()
        {
            LinkedListNode<IfStatement> current = LastIfStatementNode;
            while (current != null)
            {
                current.Value.LeftCurlyBraces++;
                current = current.Previous;
            }
        }

        private void OnRightCurlyBraceAdded() =>
            OnRightCurlyBraceAdded(1);      

        private void OnRightCurlyBraceAdded(int number)
        {
            TryRemoveLastIfStatement();

            LinkedListNode<IfStatement> current = LastIfStatementNode; 
            while (current != null)
            {
                current.Value.RightCurlyBraces += number;
                current = current.Previous;
            }
        }

        private void CloseSingleBracesInLastBlockStatement()
        {
            TryRemoveLastIfStatement();

            int bracesToClose = FirstIfStatementNodeInLastListSingleBraces;
            for (int i = 0; i < bracesToClose; i++)
                AddStatement("}", insertionCode);
        }

        private void RemoveLastBlockStatement()
        {
            CloseSingleBracesInLastBlockStatement();
            ifStatements.RemoveLast();
        }

        private void ClearLastBlockStatement()
        {
            CloseSingleBracesInLastBlockStatement();            
            ifStatements.Last.Value.Clear();
        }

        private void InsertLeftCurlyBrace()
        {
            AddStatement("{", insertionCode);
            OnLeftCurlyBraceAdded();
        }

        private void InsertRightCurlyBraces()
        {
            int bracesToClose = FindLastIfWithUnknownStatus().Value.SingleBraces;
            for (int i = 0; i < bracesToClose; i++)
                AddStatement("}", insertionCode);

            OnRightCurlyBraceAdded(bracesToClose);
        }


        private void HandleIfIf(int position)
        {
            if (ContainsSemicolons(AfterLastStatementPosition, position))
                ClearLastBlockStatement();
            else
                InsertLeftCurlyBrace();
        }

        private void HandleElseIf(int position)
        {
            if (ContainsNonSpaceCharacters(AfterLastStatementPosition, position))
                ClearLastBlockStatement();
            else
                InsertLeftCurlyBrace();
        }
      

        private bool ContainsSemicolons(int start, int end)
        {
            Regex semicolonRegex = new Regex(@";");
            Match match = semicolonRegex.Match(text, start, end - start);
            return match.Success;
        }

        private bool ContainsNonSpaceCharacters(int start, int end)
        {
            Regex nonSpaceRegex = new Regex(@"\S");
            Match match = nonSpaceRegex.Match(text, start, end - start);
            return match.Success;
        }


        private void PrintIfs()
        {
            Console.WriteLine("PRINT IF STATEMENTS");
            int a = 0;
            foreach (LinkedList<IfStatement> list in ifStatements)
            {
                Console.Write(a + "\t");
                foreach (IfStatement i in list)
                    Console.Write(i.Status + " " + i.LeftCurlyBraces + " " + i.RightCurlyBraces + " " + i.SingleBraces + "\t");
                Console.WriteLine();
                a++;
            }
            Console.WriteLine();
        }
    }
}
