using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HMS.SQLQuery //enums
{
	public enum Comparison
	{
		Equals,
		NotEquals,
		Like,
		NotLike,
		GreaterThan,
		GreaterOrEquals,
		LessThan,
		LessOrEquals,
		In
	}

	public enum JoinType
	{
		InnerJoin,
		OuterJoin,
		LeftJoin,
		RightJoin
	}

	public enum LogicOperator
	{
		And,
		Or
	}

	public enum Sorting
	{
		Ascending,
		Descending
	}

	public enum TopUnit
	{
		Records,
		Percent
	}
}
