namespace Sumo.Data.Sqlite
{
    using Microsoft.Data.Sqlite;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;

    #region delegates
    public delegate void SqliteRowUpdatedEventHandler(object sender, SqliteRowUpdatedEventArgs e);
    public delegate void SqliteRowUpdatingEventHandler(object sender, SqliteRowUpdatingEventArgs e);

    public sealed class SqliteRowUpdatedEventArgs : RowUpdatedEventArgs
    {
        public SqliteRowUpdatedEventArgs(DataRow row, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
        : base(row, command, statementType, tableMapping)
        {
        }

        new public SqliteCommand Command
        {
            get
            {
                return (SqliteCommand)base.Command;
            }
        }
    }

    public sealed class SqliteRowUpdatingEventArgs : RowUpdatingEventArgs
    {

        public SqliteRowUpdatingEventArgs(DataRow row, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
        : base(row, command, statementType, tableMapping)
        {
        }

        new public SqliteCommand Command
        {
            get { return (base.Command as SqliteCommand); }
            set { base.Command = value; }
        }

        override protected IDbCommand BaseCommand
        {
            get { return base.BaseCommand; }
            set { base.BaseCommand = (value as SqliteCommand); }
        }
    }
    #endregion

    [DefaultEvent("RowUpdated")]
    public sealed class SqliteDataAdapter : DbDataAdapter, IDbDataAdapter, ICloneable
    {
        #region ctor
        public SqliteDataAdapter() : base()
        {
            GC.SuppressFinalize(this);
        }

        public SqliteDataAdapter(SqliteCommand selectCommand) : this()
        {
            SelectCommand = selectCommand;
        }

        private SqliteDataAdapter(SqliteDataAdapter adapter) : base(adapter)
        {   // Clone
            GC.SuppressFinalize(this);
        }

        object ICloneable.Clone()
        {
            return new SqliteDataAdapter(this);
        }

        #endregion

        #region properties

        new public SqliteCommand DeleteCommand { get; set; } = null;
        IDbCommand IDbDataAdapter.DeleteCommand
        {
            get { return DeleteCommand; }
            set { DeleteCommand = (SqliteCommand)value; }
        }

        new public SqliteCommand InsertCommand { get; set; } = null;
        IDbCommand IDbDataAdapter.InsertCommand
        {
            get { return InsertCommand; }
            set { InsertCommand = (SqliteCommand)value; }
        }

        new public SqliteCommand SelectCommand { get; set; } = null;
        IDbCommand IDbDataAdapter.SelectCommand
        {
            get { return SelectCommand; }
            set { SelectCommand = (SqliteCommand)value; }
        }

        [DefaultValue(null)]
        new public SqliteCommand UpdateCommand { get; set; }
        IDbCommand IDbDataAdapter.UpdateCommand
        {
            get { return UpdateCommand; }
            set { UpdateCommand = (SqliteCommand)value; }
        }

        #endregion

        #region batch support
        override protected int AddToBatch(IDbCommand command)
        {
            throw new NotSupportedException(nameof(AddToBatch));
        }

        override protected void ClearBatch()
        {
            throw new NotSupportedException(nameof(ClearBatch));
        }

        override protected int ExecuteBatch()
        {
            throw new NotSupportedException(nameof(ExecuteBatch));
        }

        override protected IDataParameter GetBatchedParameter(int commandIdentifier, int parameterIndex)
        {
            throw new NotSupportedException(nameof(GetBatchedParameter));

        }

        override protected bool GetBatchedRecordsAffected(int commandIdentifier, out int recordsAffected, out Exception error)
        {
            throw new NotSupportedException(nameof(GetBatchedRecordsAffected));
        }

        override protected void InitializeBatching()
        {
            throw new NotSupportedException(nameof(InitializeBatching));
        }

        override protected void TerminateBatching()
        {
            throw new NotSupportedException(nameof(TerminateBatching));
        }
        #endregion

        #region events

        static private readonly object _eventRowUpdated = new object();
        static private readonly object _eventRowUpdating = new object();

        public event SqliteRowUpdatedEventHandler RowUpdated
        {
            add
            {
                Events.AddHandler(_eventRowUpdated, value);
            }
            remove
            {
                Events.RemoveHandler(_eventRowUpdated, value);
            }
        }

        public event SqliteRowUpdatingEventHandler RowUpdating
        {
            add
            {
                SqliteRowUpdatingEventHandler handler = (SqliteRowUpdatingEventHandler)Events[_eventRowUpdating];

                // MDAC 58177, 64513
                // prevent someone from registering two different command builders on the adapter by
                // silently removing the old one
                if ((null != handler) && (value.Target is DbCommandBuilder))
                {
                    SqliteRowUpdatingEventHandler d = (SqliteRowUpdatingEventHandler)FindBuilder(handler);
                    if (null != d)
                    {
                        Events.RemoveHandler(_eventRowUpdating, d);
                    }
                }
                Events.AddHandler(_eventRowUpdating, value);
            }
            remove
            {
                Events.RemoveHandler(_eventRowUpdating, value);
            }
        }

        static internal Delegate FindBuilder(MulticastDelegate mcd)
        { // V1.2.3300
            if (null != mcd)
            {
                Delegate[] d = mcd.GetInvocationList();
                for (int i = 0; i < d.Length; i++)
                {
                    if (d[i].Target is DbCommandBuilder)
                        return d[i];
                }
            }

            return null;
        }

        override protected RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
        {
            return new SqliteRowUpdatedEventArgs(dataRow, command, statementType, tableMapping);
        }

        override protected RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
        {
            return new SqliteRowUpdatingEventArgs(dataRow, command, statementType, tableMapping);
        }

        override protected void OnRowUpdated(RowUpdatedEventArgs value)
        {
            SqliteRowUpdatedEventHandler handler = (SqliteRowUpdatedEventHandler)Events[_eventRowUpdated];
            if ((null != handler) && (value is SqliteRowUpdatedEventArgs))
            {
                handler(this, (SqliteRowUpdatedEventArgs)value);
            }
            base.OnRowUpdated(value);
        }

        override protected void OnRowUpdating(RowUpdatingEventArgs value)
        {
            SqliteRowUpdatingEventHandler handler = (SqliteRowUpdatingEventHandler)Events[_eventRowUpdating];
            if ((null != handler) && (value is SqliteRowUpdatingEventArgs))
            {
                handler(this, (SqliteRowUpdatingEventArgs)value);
            }
            base.OnRowUpdating(value);
        }
        #endregion
    }
}
