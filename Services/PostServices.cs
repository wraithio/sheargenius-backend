using Microsoft.EntityFrameworkCore;
using sheargenius_backend.Context;
using sheargenius_backend.Models;
using sheargenius_backend.Models.DTOs;

namespace sheargenius_backend.Services
{
    public class PostServices
    {
        private readonly DataContext _dataContext;

        public PostServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<PostModel>> GetPostsAsync() => await _dataContext.Posts.ToListAsync();
        public async Task<List<CommentModel>> GetCommentsAsync() => await _dataContext.Comments.ToListAsync();

        public async Task<bool> AddPostAsync(PostModel post)
        {
            Console.WriteLine(post);
            await _dataContext.Posts.AddAsync(post);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var postToDelete = await GetPostByIdAsync(id);
            if (postToDelete == null) return false;
            postToDelete.IsDeleted = true;
            _dataContext.Posts.Update(postToDelete);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> AdminDeletePostAsync(int id)
        {
            var postToDelete = await GetPostByIdAsync(id);
            if (postToDelete == null) return false;
            _dataContext.Posts.Remove(postToDelete);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> AddCommentAsync(CommentModel comment)
        {
            Console.WriteLine(comment);
            PostModel postToComment = await GetPostByIdAsync(comment.postId);
            if (postToComment == null) return false;
            if (postToComment.Comments == null)
                postToComment.Comments = [];
            postToComment.Comments.Add(comment);
            _dataContext.Posts.Update(postToComment);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var postToDelete = await GetCommentByIdAsync(id);
            if (postToDelete == null) return false;
            _dataContext.Comments.Remove(postToDelete);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> ToggleLikesAsync(int postId, string username)
        {
            var postToLike = await GetPostByIdAsync(postId);
            if (postToLike == null) return false;
            if (postToLike.Likes == null) postToLike.Likes = new List<string>();
            if (postToLike.Likes.Contains(username))
            {
                postToLike.Likes = postToLike.Likes.Where(o => o != username).ToList();
            }
            else
            {
                postToLike.Likes.Add(username);
            }
                _dataContext.Posts.Update(postToLike);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<PostModel> GetPostById(int id) => await GetPostByIdAsync(id);
        public async Task<bool> EditPostAsync(PostModel post)
        {
            var postToEdit = await GetPostByIdAsync(post.Id);
            if (postToEdit == null) return false;
            postToEdit.Caption = post.Caption;
            postToEdit.PublisherName = post.PublisherName;
            postToEdit.Image = post.Image;
            postToEdit.Category = post.Category;
            postToEdit.Date = post.Date;
            postToEdit.IsDeleted = post.IsDeleted;
            postToEdit.IsPublished = post.IsPublished;
            postToEdit.Comments = post.Comments;
            // no need for await or async function for Update
            _dataContext.Posts.Update(postToEdit);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        // FindAsync searches by the primary key (aka our id) we use this over SingleOrDefaultAsync bc it is more effecient
        private async Task<CommentModel> GetCommentByIdAsync(int id) => await _dataContext.Comments.FindAsync(id);
        private async Task<PostModel> GetPostByIdAsync(int id) => await _dataContext.Posts.SingleOrDefaultAsync(post => post.Id == id);
        private async Task<PostModel> GetPostByIdCommentsAsync(int id) => await _dataContext.Posts.AsNoTracking().Include(m => m.Comments).FirstOrDefaultAsync(m => m.Id == id);

        public async Task<List<PostModel>> GetPostsByUserIdAsync(int id) => await _dataContext.Posts.Where(posts => posts.UserId == id && posts.IsDeleted == false && posts.IsPublished == true).ToListAsync();

        public async Task<List<CommentModel>> GetCommentsByPostIdAsync(int id) => await _dataContext.Comments.Where(comment => comment.postId == id).ToListAsync();

        public async Task<IEnumerable<PostModel>> GetPostsbyCategory(string category) => await _dataContext.Posts.Where(posts => posts.Category == category && posts.IsDeleted == false && posts.IsPublished == true).ToListAsync();


        private async Task<UserModel> GetUserByUsername(string username) => await _dataContext.Users.SingleOrDefaultAsync(user => user.Username == username);

    }
}