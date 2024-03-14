﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AquariumMVC.Models;

public partial class Memberdata
{
    [Key]
    [StringLength(30)]
    [Unicode(false)]
    public string Account { get; set; }

    [Required]
    [StringLength(30)]
    [Unicode(false)]
    public string Password { get; set; }

    [Required]
    [StringLength(30)]
    public string Name { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string Email { get; set; }

    [Required]
    public string Address { get; set; }

    public bool IsAdmin { get; set; }
}