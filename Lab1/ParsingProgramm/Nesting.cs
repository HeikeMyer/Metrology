using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB
{
    public class Nesting
    {
        private string[] _program;
        private Stack<int> _if_nestings;
        private Stack<int> _opened_braces_nestings;
        private int _length;
        private Stack<Tuple<string, int>> _if_for_else_stack;
        private int _last_nesting;

        public Nesting(string[] program)
        {
            _program = program;
            _length = _program.Length;
            _if_nestings = new Stack<int>();
            _opened_braces_nestings = new Stack<int>();
            _last_nesting = 0;
            _if_for_else_stack = new Stack<Tuple<string, int>>();
        }

        public int MaxNesting()
        {
            FillInNestings();
            var nest_arr = _if_nestings.ToArray();
            try
            {
                return nest_arr.Max();

            }
            catch (ArgumentNullException)
            {
                return 0;
            }
        }

        private void FillInNestings()
        {
            int i = 0;
            while (_program[i] != null)
            {
                switch (_program[i])
                {
                    case "if":
                        SetIfNesting(i);
                        break;
                    case "else":
                        SetElseNesting(i);
                        break;
                    case "{":
                        SetOpenedBraceNesting(i);
                        break;
                    case "}":
                        SetClosedBraceNesting(i);
                        break;
                    default:
                        throw new Exception("wrong input");
                }

                i++;
            }
        }

        private void SetIfNesting(int index)
        {
            int nesting;
            if (index == 0)
            {
                nesting = 0;
            }
            else
            {
                switch (_program[index - 1])
                {
                    case "{":

                        {
                            nesting = _last_nesting + 1;
                            break;
                        }
                    case "}":
                    case "else"://это не elseif ,т.к иначе парсер поставил бы туда скобку,это новый if,наподобие if oper; else oper; if oper;
                    case "if"://это не вложенный if,т.к для вложенного ставится открывающая скобка
                        {
                            if (_opened_braces_nestings.Count != 0)
                            {
                                nesting = _opened_braces_nestings.Peek() + 1;//вложенность последней открытой скобки + 1
                            }
                            else
                            {
                                nesting = 0;
                            }
                            break;
                        }
                    default:
                        {
                            throw new Exception("wrong input");
                        }
                }
            }

            _if_nestings.Push(nesting);
            _if_for_else_stack.Push(new Tuple<string, int>("if", nesting));
            _last_nesting = nesting;
        }

        private void SetOpenedBraceNesting(int index)
        {
            int nesting;
            if (index != 0)
            {
                nesting = _last_nesting;
            }
            else
            {
                nesting = -1;
            }
            _opened_braces_nestings.Push(nesting);//в стек вложенностей открытых скобок помещаем новое значение
            _if_for_else_stack.Push(new Tuple<string, int>("{", nesting));//в стек if,которые претендуют на else
            _last_nesting = nesting;
        }

        private void SetClosedBraceNesting(int index)
        {
            int nesting = _opened_braces_nestings.Pop();//если нет соответствующей открывающей скобки-исключение
            while (_if_for_else_stack.Pop().Item1 != "{") ;//убираем из стека if-ов,претендующих на else все if-ы,которые будут заключены в скобки,наподобие {if if}
            _last_nesting = nesting;
        }

        private void SetElseNesting(int index)
        {
            var if_element = _if_for_else_stack.Pop();
            if (if_element.Item1 != "if")
            {
                throw new Exception("wrong input");
            }
            _last_nesting = if_element.Item2;
        }

    }
}
