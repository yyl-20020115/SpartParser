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
        protected Rule group = new Rule(nameof(group));
        protected Rule term = new Rule(nameof(term));
        protected Rule factor = new Rule(nameof(factor));
        protected Rule expression = new Rule(nameof(expression));
        protected Rule integer = new Rule(nameof(integer));

        /// <summary>
        /// A very simple calculator parser
        /// </summary>
        public Calculator()
        {
            // creating sub parsers
            Parser add = '+' + term;
            // attaching semantic action
            add.Action += new ActionHandler(
                (parser, args) =>
                {
                    Console.WriteLine("+");
                }
            );

            Parser sub = '-' + term;
            sub.Action += new ActionHandler(
                (parser, args) =>
                {
                    Console.WriteLine("-");
                }
            );

            Parser mul = '*' + factor;
            mul.Action += new ActionHandler(
                (parser, args) =>
                {
                    Console.WriteLine("*");
                }
            );

            Parser div = '/' + factor;
            div.Action += new ActionHandler(
                (parser, args) =>
                {
                    Console.WriteLine("/");
                }
            );

            // assigning parsers to rules
            integer.Parser = +Prims.Digit;
            integer.Action += new ActionHandler(
                (parser, args) =>
                {
                    Console.WriteLine($"integer: {args.Value}");
                }
            );

            group.Parser = '(' + expression + ')';
            group.Action += new ActionHandler(
                (parser, args) =>
                {
                    Console.WriteLine($"group: {args.Value}");
                }
            );

            factor.Parser = group | integer;
            factor.Action += new ActionHandler(
                (parser, args) =>
                {
                    Console.WriteLine($"factor: {args.Value}");
                }
            );

            term.Parser = factor + ~(mul | div);
            term.Action += new ActionHandler(
                (parser, args) =>
                {
                    Console.WriteLine($"term: {args.Value}");
                }
            );
            expression.Parser = term + ~(add | mul);
            expression.Action += new ActionHandler(
                (parser, args) =>
                {
                    Console.WriteLine($"expression: {args.Value}");
                }
            );
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
