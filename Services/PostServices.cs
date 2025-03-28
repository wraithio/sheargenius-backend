using Microsoft.EntityFrameworkCore;
using sheargenius_backend.Context;
using sheargenius_backend.Models;

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

        public async Task<bool> AddPostAsync(PostModel post)
        {
            await _dataContext.Posts.AddAsync(post);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> EditPostAsync(PostModel post)
        {
            var postToEdit = await GetPostByIdAsync(post.Id);
            if(postToEdit == null) return false;
            postToEdit.Caption = post.Caption;
            postToEdit.PublisherName = post.PublisherName;
            postToEdit.Image = post.Image;
            postToEdit.Category = post.Category;
            postToEdit.Date = post.Date;
            postToEdit.IsDeleted = post.IsDeleted;
            postToEdit.IsPublished = post.IsPublished;
            // no need for await or async function for Update
            _dataContext.Posts.Update(postToEdit);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        // FindAsync searches by the primary key (aka our id) we use this over SingleOrDefaultAsync bc it is more effecient
        private async Task<PostModel> GetPostByIdAsync(int id) => await _dataContext.Posts.FindAsync(id);

        public async Task<List<PostModel>> GetPostsByUserIdAsync(int id) => await _dataContext.Posts.Where(posts => posts.UserId == id).ToListAsync();

        public async Task<List<PostModel>> GetPostsbyCategory(string category) => await _dataContext.Posts.Where(posts => posts.Category == category).ToListAsync();
        
        
    }
}