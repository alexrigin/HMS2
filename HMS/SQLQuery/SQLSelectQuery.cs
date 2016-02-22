using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using HMS.SQLQuery;

namespace HMS.SQLQuery
{
	class SQLSelectQuery
	{
		private string _query;

		private bool _isDistinct = false;
		private List<string> _selectedColumns = new List<string>();	
		private List<string> _selectedTables = new List<string>();	//From  
		private WhereStatement _whereStatement = new WhereStatement();
		private List<OrderByClause> _orderByStatement = new List<OrderByClause>();
		private List<string> _groupByColumns = new List<string>();
		private WhereStatement _havingStatement = new WhereStatement();
		private LimitClause _limitStatement = null;

		public SQLSelectQuery() { }
		public SQLSelectQuery(SQLSelectQuery query)
		{

			this.IsDistinct = query.IsDistinct;

			foreach(string column in query._selectedColumns) {
				this._selectedColumns.Add(column);
			}

			foreach (string column in query._selectedTables) {
				this._selectedTables.Add(column);
			}

			this._whereStatement = WhereStatement.Copy(query._whereStatement);

			foreach (OrderByClause clause in query._orderByStatement) {
				this._orderByStatement.Add(clause);
			}

			foreach (string column in query._groupByColumns) {
				this._groupByColumns.Add(column);
			}

			this._havingStatement = WhereStatement.Copy(query._havingStatement);

			if (query._limitStatement != null)
				this._limitStatement = new LimitClause(query._limitStatement.From, query._limitStatement.To);
		}

		public string  Query { get { return _query; } }
		public bool IsDistinct { get { return _isDistinct; } set { _isDistinct = value; } }
		public WhereStatement Where { get { return _whereStatement; } set { _whereStatement = value; } }
		public WhereStatement Having { get { return _havingStatement; } set { _havingStatement = value; } }
		public List<OrderByClause> OrderBy { get { return _orderByStatement; } set { _orderByStatement = value; } }
		public List<string> GroupBy { get { return _groupByColumns; } set { _groupByColumns = value; } }
		public LimitClause Limit { get { return _limitStatement; } set { _limitStatement = value; } }
		public List<string> SelectedTables { get { return _selectedTables; } set { _selectedTables = value; } }
		public List<string> SelectedColumns { get { return _selectedColumns; } set { _selectedColumns = value; } }
		//public List<string> SelectedColumns
		//{
		//	get
		//	{
		//		if (_selectedColumns.Count == 0)
		//			_selectedColumns.Add("*");
		//		return _selectedColumns;
		//	}
		//}

		/// <summary>
		/// Add column name in Select statement.
		/// (Reset all selected columns)
		/// </summary>
		/// <param name="column"></param>
		public void SelectColumn(string column)
		{
			_selectedColumns.Clear();
			_selectedColumns.Add(column);
		}
		/// <summary>
		/// Add columns names in Select statement.
		/// (Reset all selected columns)
		/// </summary>
		/// <param name="columns"></param>
		public void SelectColumns(params string[] columns)
		{
			_selectedColumns.Clear();
			foreach (string column in columns) {
				_selectedColumns.Add(column);
			}
		}

		/// <summary>
		/// Add table name in Select (From part) statement.
		/// (Reset all selected tables)
		/// </summary>
		/// <param name="column"></param>
		public void SelectFromTable(string table)
		{
			_selectedTables.Clear();
			_selectedTables.Add(table);
		}
		/// <summary>
		/// Add tables names in Select (From part) statement.
		/// (Reset all selected tables)
		/// </summary>
		/// <param name="columns"></param>
		public void SelectFromTables(params string[] tables)
		{
			_selectedTables.Clear();
			foreach (string column in tables) {
				_selectedTables.Add(column);
			}
		}

		public void AddWhere(WhereClause clause) { AddWhere(clause, 1); }
		public void AddWhere(WhereClause clause, int level)
		{
			_whereStatement.Add(clause, level);
		}
		public WhereClause AddWhere(string field, Comparison @operator, object compareValue)
		{
			return AddWhere(field, @operator, compareValue, 1);
		}
		public WhereClause AddWhere(Enum field, Comparison @operator, object compareValue)
		{
			return AddWhere(field.ToString(), @operator, compareValue, 1);
		}
		public WhereClause AddWhere(string field, Comparison @operator, object compareValue, int level)
		{
			WhereClause NewWhereClause = new WhereClause(field, @operator, compareValue);
			_whereStatement.Add(NewWhereClause, level);
			return NewWhereClause;
		}

		public void AddGroupByColumn(string column)
		{
			_groupByColumns.Add(column);
		}

		public void AddGroupByColumns(params string[] columns)
		{
		foreach (string column in columns) {
				_groupByColumns.Add(column);
			}
		}

		public void AddHaving(WhereClause clause)
		{
			AddHaving(clause, 1);
		}
		public void AddHaving(WhereClause clause, int level)
		{
			_havingStatement.Add(clause, level);
		}
		public WhereClause AddHaving(string field, Comparison @operator, object compareValue)
		{
			return AddHaving(field, @operator, compareValue, 1);
		}
		public WhereClause AddHaving(Enum field, Comparison @operator, object compareValue)
		{
			return AddHaving(field.ToString(), @operator, compareValue, 1);
		}
		public WhereClause AddHaving(string field, Comparison @operator, object compareValue, int level)
		{
			WhereClause NewWhereClause = new WhereClause(field, @operator, compareValue);
			_havingStatement.Add(NewWhereClause, level);
			return NewWhereClause;
		}

		public void AddOrderBy(OrderByClause clause)
		{
			_orderByStatement.Add(clause);
		}
		public void AddOrderBy(Enum field, Sorting order)
		{
			this.AddOrderBy(field.ToString(), order);
		}
		public void AddOrderBy(string field, Sorting order)
		{
			OrderByClause NewOrderByClause = new OrderByClause(field, order);
			_orderByStatement.Add(NewOrderByClause);
		}
		public void OrderByClear()
		{
			_orderByStatement.Clear();
		}

		public void AddLimit(int from, int to)
		{
			_limitStatement = new LimitClause(from, to);
		}

		public void LimitClear()
		{
			_limitStatement = null;
		}

		public string BuildQuery()
		{
			string query = "SELECT ";
			
			// Is Dinstinct
			if (IsDistinct)
				query += "DISTINCT ";

			// Columns names
			if (_selectedColumns.Count == 0) {
				if (SelectedTables.Count == 1)
					query += _selectedTables[0] + ".";
				query += "*";
			} else {
				foreach (string column in SelectedColumns) {
					query += column + ',';
				}
				query = query.TrimEnd(',');
				query += " \n" ;
			}

			// Tables names
			if(_selectedTables.Count == 0) {
				throw new Exception("No selected tables");
			} else {
				query += " FROM ";
				foreach (string table in SelectedTables) {
					query += table + ',';
				}
				query = query.TrimEnd(',');
				query += " \n";
			}

			//Where statement
			query += " WHERE " + _whereStatement.BuildWhereStatement();
			query += " \n";	  

			//groupby statement
			if(_groupByColumns.Count > 0) {
				query += " GROUP BY ";
				foreach (string Column in _groupByColumns) {
					query += Column + ',';
				}
				query = query.TrimEnd(',');
				query += " \n";
			}

			// having statement
			if (_havingStatement.ClauseLevels > 0) {
				// Check if a Group By Clause was set
				if (_groupByColumns.Count == 0) {
					throw new Exception("Having statement was set without Group By");
				}
				query += " HAVING " + _havingStatement.BuildWhereStatement();
				query += " \n";
			}

			//order by statement
			if (_orderByStatement.Count > 0) {
				query += " ORDER BY ";
				foreach (OrderByClause Clause in _orderByStatement) {
					string OrderByClause = "";
					switch (Clause.SortOrder) {
						case Sorting.Ascending:
							OrderByClause = Clause.FieldName + " ASC"; break;
						case Sorting.Descending:
							OrderByClause = Clause.FieldName + " DESC"; break;
					}
					query += OrderByClause + ',';
				}
				query = query.TrimEnd(','); // Trim de last AND inserted by foreach loop
				query += " \n";
			}

			if (_limitStatement != null) {
				query += " LIMIT " + _limitStatement.From +" , "+ _limitStatement.To;
			}

			query += " ;";

			_query = query;
			return query;
		}

	}

}
