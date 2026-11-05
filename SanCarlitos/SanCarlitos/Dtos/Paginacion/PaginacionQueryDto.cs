using System.ComponentModel.DataAnnotations;

namespace SanCarlitos.Dtos.Paginacion
{
    public class PaginacionQueryDto
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;

    }
}
