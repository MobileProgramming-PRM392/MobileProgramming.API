﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MobileProgramming.Business.Models.DTO.Feedbacks
{
    public class CreateFeedbackDto
    {
        [Required(ErrorMessage = "Product is required")]
        public int ProductId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5)]
        public int Rating { get; set; }
        [Required(ErrorMessage = "Comment is required")]
        public string? Comment { get; set; }
    }
}