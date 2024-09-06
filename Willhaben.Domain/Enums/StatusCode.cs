namespace Willhaben.Domain.Models;

public enum StatusCode
{
    SUCCESS = 200,
    CREATED = 201,
    BAD_REQUEST = 400,
    FORBIDDEN = 403,
    NOT_FOUND = 404,
    SCRAPER_ERROR = -1
}