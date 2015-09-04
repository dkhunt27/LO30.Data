using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data
{
  public class ForWebPlayerStat
  {
    public string Player { get; set; }
    public string Team { get; set; }
    public string Sub { get; set; }
    public string Pos { get; set; }
    public int Line { get; set; }
    public int GP { get; set; }
    public int G { get; set; }
    public int A { get; set; }
    public int P { get; set; }
    public int PIM { get; set; }
    public int PPG { get; set; }
    public int SHG { get; set; }
    public int GWG { get; set; }
  }
}