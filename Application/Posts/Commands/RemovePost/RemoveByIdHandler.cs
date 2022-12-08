﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggr.Application.Posts.Commands.RemovePost
{
    public class RemoveByIdHandler : IRequestHandler<RemoveByIdCommand, Post>
    {
        private readonly IUnitOfWork _UOW;

        public RemoveByIdHandler(IUnitOfWork UOW)
        {
            _UOW = UOW;
        }
        public async Task<Post?> Handle(RemoveByIdCommand request, CancellationToken cancellationToken)
        {
            var result = await _UOW.Posts.RemoveById(request.id);
            await _UOW.Save();
            return result;
        }
    }
}
