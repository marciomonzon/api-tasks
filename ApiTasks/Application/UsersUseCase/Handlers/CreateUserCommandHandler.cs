using Application.Response;
using Application.UsersUseCase.Commands;
using Application.UsersUseCase.ViewModels;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;

namespace Application.UsersUseCase.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseBase<UserInfoViewModel>>
    {
        private readonly TasksDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(TasksDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResponseBase<UserInfoViewModel>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var userInfo = new ResponseBase<UserInfoViewModel>()
            {
                ResponseInfo = null,
                Value = _mapper.Map<UserInfoViewModel>(user)
            };

            return userInfo;
        }
    }
}
