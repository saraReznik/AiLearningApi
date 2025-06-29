﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Dal.Models;

public partial class User
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }
    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string Email { get; set; } = null!;

    public string? Role { get; set; }

    public virtual ICollection<Prompt> Prompts { get; set; } = new List<Prompt>();
}
