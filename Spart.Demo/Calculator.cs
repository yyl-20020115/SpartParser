using System;

namespace Spart.Demo
{
    using Spart.Parsers.NonTerminal;
    using Spart.Parsers;
    using Spart.Scanners;
    using Spart.Actions;

    /// <summary>
    /// group       ::= '(' expression ')'
    /// factor      ::= integer | group
    /// term        ::= factor (('*' factor) | ('/' factor))*
    /// expression  ::= term (('+' term) | ('-' term))*
    /// </summary>
    public class Calculator
    {
        protected Rule group;
        protected Rule term;
        protected Rule factor;
        protected Rule expression;
        protected Rule integer;

        /// <summary>
        /// A very simple calculator parser
        /// </summary>
        public Calculator()
        {
            // creating rules and assigning names
            group = new Rule("group");

            term = new Rule("term");

            factor = new Rule("factor");

            expression = new Rule("expression");

            integer = new Rule("integer");

            // creating sub parsers
            Parser add = '+' + term;
            // attaching semantic action
            add.Act += new ActionHandler((o, args) => Console.WriteLine("add"));

            Parser sub = '-' + term;
            sub.Act += new ActionHandler((o, args) => Console.WriteLine("sub"));

            Parser mul = '*' + factor;
            mul.Act += new ActionHandler((o, args) => Console.WriteLine("mul"));

            Parser div = '/' + factor;
            div.Act += new ActionHandler((o, args) => Console.WriteLine("div"));

            // assigning parsers to rules
            integer.Parser = Prims.Digit;

            group.Parser = '(' + expression + ')';

            factor.Parser = group | integer;

            term.Parser = factor + ~(mul | div);

            expression.Parser = term + ~(add | mul);
        }

        /// <summary>
        /// Parse a string and return parse match
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public virtual ParserMatch Parse(string s)
        {
            return expression.Parse(new StringScanner(s));
        }
    }
}
