using AutoMapper;
using Bloggr.Application.Models;
using Bloggr.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggr.Application.Comments.Queries.GetPostComments
{
    public class GetPostCommentsHandler : IRequestHandler<GetPostCommentsQuery, PagedResultDto<CommentQueryDto>>
    {
        private readonly IUnitOfWork _UOW;
        private readonly IMapper _mapper;

        public GetPostCommentsHandler(IUnitOfWork UOW, IMapper mapper)
        {
            _UOW = UOW;
            _mapper = mapper;
        }

        public async Task<PagedResultDto<CommentQueryDto>> Handle(GetPostCommentsQuery request, CancellationToken cancellationToken)
        {
            var query = _UOW.Comments.Query().Where(comment => comment.PostId == request.postId).Include(comment => comment.User);
            var pagedResult = await _UOW.Comments.Paginate(query, request.pageDto);
            var mappedReuslt = PagedResultDto<CommentQueryDto>.From<Comment>(pagedResult, _mapper);
            return mappedReuslt;
        }
    }
}
