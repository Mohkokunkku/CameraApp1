public interface IObservation
{
    int pkId { get; set; }
    string absolutepath { get; set; }
    string imageuri { get; set; }
    string observation { get; set; }
    string visitguid { get; set; }
    string cachepath { get; set; }
}