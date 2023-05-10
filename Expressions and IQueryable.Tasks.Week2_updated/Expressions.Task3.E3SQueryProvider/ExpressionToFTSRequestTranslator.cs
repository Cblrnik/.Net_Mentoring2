using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;

        public ExpressionToFtsRequestTranslator()
        {
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);

            return _resultStringBuilder.ToString();
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            switch (node.Method.Name)
            {
                case "Where":
                    var predicate = node.Arguments[1];
                    Visit(predicate);

                    return node;
                case "Equals":
                    var argument = node.Arguments[0];

                    Visit(node.Object);
                    _resultStringBuilder.Append("(");
                    Visit(argument);
                    _resultStringBuilder.Append(")");
                    return node;
                case "Contains":
                    var containsArgument = node.Arguments[0];

                    Visit(node.Object);
                    _resultStringBuilder.Append("(*");
                    Visit(containsArgument);
                    _resultStringBuilder.Append("*)");
                    return node;
                case "StartsWith":
                    var startWithArgument = node.Arguments[0];

                    Visit(node.Object);
                    _resultStringBuilder.Append("(");
                    Visit(startWithArgument);
                    _resultStringBuilder.Append("*)");
                    return node;

                case "EndsWith":
                    var endsWithArgument = node.Arguments[0];

                    Visit(node.Object);
                    _resultStringBuilder.Append("(*");
                    Visit(endsWithArgument);
                    _resultStringBuilder.Append(")");
                    return node;
                default:
                    return base.VisitMethodCall(node);
            }
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    if (node.Left.NodeType != ExpressionType.MemberAccess)
                    {
                        Visit(node.Right);
                        _resultStringBuilder.Append("(");
                        Visit(node.Left);
                        _resultStringBuilder.Append(")");
                        break;
                    }

                    Visit(node.Left);
                    _resultStringBuilder.Append("(");
                    Visit(node.Right);
                    _resultStringBuilder.Append(")");
                    break;

                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    Visit(node.Left);
                    _resultStringBuilder.Append("\r\n");
                    Visit(node.Right);
                    break;
                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append(node.Member.Name).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultStringBuilder.Append(node.Value);

            return node;
        }

        #endregion
    }
}
