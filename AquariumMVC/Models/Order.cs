﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AquariumMVC.Models;

public partial class Order
{
    [Key]
    public int O_id { get; set; }

    [StringLength(50)]
    public string OrderGuid { get; set; }

    [StringLength(30)]
    public string Account { get; set; }

    [StringLength(30)]
    public string Receiver { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string ReceiverTel { get; set; }

    [StringLength(50)]
    public string Address { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Date { get; set; }

    public int? total_price { get; set; }
}