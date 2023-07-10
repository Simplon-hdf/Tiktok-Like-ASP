using System.ComponentModel.DataAnnotations.Schema;

namespace TiktokLikeASP.Models
{
    public class Follow
    {
        [Column("follower_id")]
        public int FollowerId { get; set; }

        [Column("user_followed_id")]
        public int userFollowedId { get; set; }

        [Column("follower")]
        public User Follower { get; set; }

        [Column("user_followed")]
        public User UserFollowed { get; set; }
    }
}
