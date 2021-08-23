using System.Text;

namespace CSharpier
{
    public class DisabledTextComparer
    {
        // when we encounter disabled text, it is inside an #if when the condition on the #if is false
        // we format all that code by doing multiple passes of csharpier
        // SyntaxNodeComparer is very slow, and running it after every pass on a file would take a long time
        // so we do this quick version of validating that the code is basically the same besides
        // line breaks and spaces
        public static bool IsCodeBasicallyEqual(string code, string formattedCode)
        {
            var squashCode = Squash(code);
            var squashFormattedCode = Squash(formattedCode);

            var result = squashCode == squashFormattedCode;
            return result;
        }

        protected static string Squash(string code)
        {
            var result = new StringBuilder();
            for (var index = 0; index < code.Length; index++)
            {
                var nextChar = code[index];
                if (nextChar is ' ' or '\t' or '\r' or '\n')
                {
                    if (
                        result.Length > 0
                        && index != code.Length - 1
                        && result[^1]
                            is not (' '
                                or '('
                                or ')'
                                or '{'
                                or '}'
                                or '['
                                or ']'
                                or '.'
                                or ','
                                or '*'
                                or '+'
                                or '-'
                                or '='
                                or '/'
                                or '%'
                                or '|'
                                or '&'
                                or '!'
                                or '<'
                                or '>'
                                or ':'
                                or ';')
                    ) {
                        result.Append(' ');
                    }
                }
                else if (
                    nextChar
                        is '('
                            or ')'
                            or '{'
                            or '}'
                            or ']'
                            or '['
                            or '.'
                            or ','
                            or '*'
                            or '+'
                            or '-'
                            or '='
                            or '/'
                            or '%'
                            or '|'
                            or '&'
                            or '!'
                            or '<'
                            or '>'
                            or ';'
                            or ':'
                    && result.Length > 0
                    && result[^1] is ' '
                ) {
                    result[^1] = nextChar;
                }
                else
                {
                    result.Append(nextChar);
                }
            }

            return result.ToString();
        }
    }
}
