﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Room_Scheduling_Software.Data.Entities
{
    [Table("Rooms")]
    public class Room
    {
        #region < PROPERTIES >

        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Capacity { get; set; }

        public decimal Hourly_Price { get; set; }

        public bool IsFree { get; set; } = true;

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        #endregion

        #region < CONSTRUCTORS >

        public Room() { }

        public Room (int categoryID, string name, int capacity, decimal hourly_price, bool isFree)
        {
            this.Name = name;
            this.Capacity = capacity;
            this.Hourly_Price = hourly_price;
            this.IsFree = isFree;
            this.CategoryId = categoryID;
        }

        #endregion

    }
}