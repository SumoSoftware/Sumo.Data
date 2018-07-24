namespace Sumo.Data.Schema
{
    public enum ConflictClauses
    {
        Rollback = 1,
        Abort = 2,
        Fail = 3,
        Ignore = 4,
        Replace = 5
    }
}
