using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.SQLQuery //Clauses
{
	public struct OrderByClause
	{
		public string FieldName { get; set; }
		public Sorting SortOrder { get; set; }
		public OrderByClause(string field)
		{
			FieldName = field;
			SortOrder = Sorting.Ascending;
		}

		public OrderByClause(string field, Sorting order)
		{
			FieldName = field;
			SortOrder = order;
		}
	}

	/// <summary>
	/// Represents a WHERE clause on 1 database column, containing 1 or more comparisons on 
	/// that column, chained together by logic operators: eg (UserID=1 or UserID=2 or UserID>100)
	/// This can be achieved by doing this:
	/// WhereClause myWhereClause = new WhereClause("UserID", Comparison.Equals, 1);
	/// myWhereClause.AddClause(LogicOperator.Or, Comparison.Equals, 2);
	/// myWhereClause.AddClause(LogicOperator.Or, Comparison.GreaterThan, 100);
	/// </summary>
	public struct WhereClause
	{
		public string FieldName { get; set; }
		public Comparison ComparisonOperator { get; set; }
		public object Value { get; set; }

		internal struct SubClause
		{
			public LogicOperator LogicOperator { get; set; }
			public Comparison ComparisonOperator { get; set; }
			public object Value { get; set; }
			public SubClause(LogicOperator logic, Comparison compareOperator, object compareValue)
			{
				LogicOperator = logic;
				ComparisonOperator = compareOperator;
				Value = compareValue;
			}
		}

		internal List<SubClause> SubClauses;    // Array of SubClause

		public WhereClause(string field, Comparison firstCompareOperator, object firstCompareValue)
		{
			FieldName = field;
			ComparisonOperator = firstCompareOperator;
			Value = firstCompareValue;
			SubClauses = new List<SubClause>();
		}

		public void AddClause(LogicOperator logic, Comparison compareOperator, object compareValue)
		{
			SubClause NewSubClause = new SubClause(logic, compareOperator, compareValue);
			SubClauses.Add(NewSubClause);
		}
	}

	

	public class SqlLiteral
	{
		public static string StatementRowsAffected = "SELECT @@ROWCOUNT";

		private string _value;
		public string Value
		{
			get { return _value; }
			set { _value = value; }
		}

		public SqlLiteral(string value)
		{
			_value = value;
		}
	}

	public class LimitClause
	{
		public int From { get; set; }
		public int To { get; set; }

		public LimitClause(int from, int to)
		{
			From = from;
			To = to;
		}
	}
}
