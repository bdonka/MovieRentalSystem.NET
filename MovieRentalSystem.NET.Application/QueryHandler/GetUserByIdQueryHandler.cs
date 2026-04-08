using MediatR;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Query;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IGenreService _genreService;
    public GetUserByIdQueryHandler(IGenreService genreService)
    {
        _genreService = genreService;
    }

    public async Task<UserResponse> Handle(
        GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _genreService.GetByIdAsync(request.Id);
    }
}