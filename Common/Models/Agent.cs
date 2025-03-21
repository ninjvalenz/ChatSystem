using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Agent
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Seniority { get; set; }
    public int CurrentChats { get; set; } = 0;
    public int MaxChats { get; set; }
    public TimeSpan ShiftStart { get; set; }

    public TimeSpan ShiftEnd { get; set; }
}