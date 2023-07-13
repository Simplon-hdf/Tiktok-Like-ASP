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
        public DateOnly PublishDate { get; set; }

        [Column("is_visible")]
        public bool IsVisible { get; set; }

        /*         RELATIONS         */

        [Column("user_likes")]
        public List<Person> UserLikes { get; set; }

        [Column("tags")]
        public List<Tag> Tags { get; set; }
        
        [Column("creator_id")]
        public Guid CreatorId { get; set; }

        [Column("creator")]
        public Person Creator { get; set; }

        [Column("comments")]
        public List<Comment> Comments { get; set; }
    }
}
