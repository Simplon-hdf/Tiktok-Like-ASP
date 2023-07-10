﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiktokLikeASP.Models
{
    public class Post
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("video_link")]
        public string VideoLink { get; set; }

        [Column("publish_date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PublishDate { get; set; }

        [Column("is_visible")]
        public bool IsVisible { get; set; }

        public List<User> Userlikes { get; set; }


        /*         RELATIONS         */
        [Column("tags")]
        public List<Tag> Tags { get; set; }

        [Column("users")]
        public User Users { get; set; }

        [Column("comments")]
        public List<Comment> Comments { get; set; }
    }
}
