namespace Sumo.Data.Generator.Models
{
    public class Manifest
    {
        public int index { get; set; }
        public string table_name { get; set; }
        public string primary_key { get; set; }
        public int record_count { get; set; }
        public bool full_sync{ get; set; }
    }
}
