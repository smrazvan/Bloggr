using AutoMapper;
using Bloggr.Application.Interests.Queries.GetInterests;
using Bloggr.Application.Models;
using Bloggr.Infrastructure.Interfaces;
using Bloggr.Infrastructure.Services;
using Domain.Abstracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggr.Application.Posts.Queries.GetPosts
{
    public class GetPostsHandler : IRequestHandler<GetPostsQuery, PagedResultDto<PostsQueryDto>>
    {
        private readonly IUnitOfWork _UOW;
        private readonly IMapper _mapper;

        public GetPostsHandler(IUnitOfWork UOW, IMapper mapper)
        {
            _UOW = UOW;
            _mapper = mapper;
        }
        public async Task<PagedResultDto<PostsQueryDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {

            //filtering
            var query = _UOW.Posts.Query();
            if (request.userId is not null)
            {
                query = query.Where(post => post.UserId == request.userId);
            }
            if(!string.IsNullOrEmpty(request.input))
            {
                query = query.Where(post => post.Title.Contains(request.input));
            }
            if (!string.IsNullOrEmpty(request.orderBy))
            {
                if (request.orderBy == "asc")
                    query = query.OrderBy(post => post.CreationDate);
                else if (request.orderBy == "desc")
                    query = query.OrderByDescending(post => post.CreationDate);
            }
            if (request.interests.Any())
            {
                //.All(interest => request.interests.Contains(interest.Name));
                query = query.Where(post => post.InterestPosts.Select(interestPost => interestPost.Interest).All(interest => request.interests.Contains(interest.Name)) && post.InterestPosts.Count() != 0);
            }
            var includeQuery = query.Include(post => post.InterestPosts).ThenInclude(interestPost => interestPost.Interest).Include(post => post.User);
            var pagedResult = await _UOW.Posts.Paginate(includeQuery, request.pageDto);
            await _UOW.Posts.SetPostListProps(pagedResult.Result);
            var mappedResult = PagedResultDto<PostsQueryDto>.From(pagedResult, _mapper);
            return mappedResult; 
        }
    }
}
