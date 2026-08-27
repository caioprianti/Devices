using Devices.Api.Contracts.Devices;
using Devices.Application.Devices.Common;
using Devices.Application.Devices.Create;
using Devices.Application.Devices.Delete;
using Devices.Application.Devices.GetAll;
using Devices.Application.Devices.GetById;
using Devices.Application.Devices.Patch;
using Devices.Application.Devices.Update;
using Microsoft.AspNetCore.Mvc;
using Devices.Api.Extensions;

namespace Devices.Api.Controllers;

/// <summary>
/// Manages device resources.
/// </summary>
[ApiController]
[Route("devices")]
[Produces("application/json")]
public sealed class DevicesController(
    CreateDeviceCommandHandler createHandler,
    GetDeviceByIdQueryHandler getByIdHandler,
    GetDevicesQueryHandler getAllHandler,
    UpdateDeviceCommandHandler updateHandler,
    PatchDeviceCommandHandler patchHandler,
    DeleteDeviceCommandHandler deleteHandler) : ControllerBase
{
    /// <summary>
    /// Creates a device.
    /// </summary>
    /// <param name="request">Device data.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <response code="201">The device was created.</response>
    /// <response code="400">The request is invalid.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CreateDeviceRequest request,
        CancellationToken cancellationToken)
    {
        var result = await createHandler.HandleAsync(
            new CreateDeviceCommand(request.Name, request.Brand, request.State),
            cancellationToken);

        if (!result.IsSuccess)
            return result.Error!.ToProblem();

        var device = result.Value!;

        return CreatedAtAction(
            nameof(GetById),
            new { id = device.Id },
            device);
    }

    /// <summary>
    /// Gets a device by identifier.
    /// </summary>
    /// <param name="id">Device identifier.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <response code="200">The device was found.</response>
    /// <response code="400">The identifier is invalid.</response>
    /// <response code="404">The device was not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await getByIdHandler.HandleAsync(
            new GetDeviceByIdQuery(id),
            cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error!.ToProblem();
    }

    /// <summary>
    /// Gets devices using optional filters.
    /// </summary>
    /// <param name="request">Optional brand and state filters.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <response code="200">The matching devices were returned.</response>
    /// <response code="400">The filters are invalid.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<DeviceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetDevicesRequest request,
        CancellationToken cancellationToken)
    {
        var result = await getAllHandler.HandleAsync(
            new GetDevicesQuery(request.Brand, request.State),
            cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value ?? [])
            : result.Error!.ToProblem();
    }

    /// <summary>
    /// Replaces a device's mutable properties.
    /// </summary>
    /// <param name="id">Device identifier.</param>
    /// <param name="request">Complete device data.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <response code="200">The device was updated.</response>
    /// <response code="400">The request is invalid.</response>
    /// <response code="404">The device was not found.</response>
    /// <response code="409">The update violates a domain rule.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpPut("{id}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateDeviceRequest request,
        CancellationToken cancellationToken)
    {
        var result = await updateHandler.HandleAsync(
            new UpdateDeviceCommand(id, request.Name, request.Brand, request.State),
            cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error!.ToProblem();
    }

    /// <summary>
    /// Partially updates a device.
    /// </summary>
    /// <param name="id">Device identifier.</param>
    /// <param name="request">Properties to update.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <response code="200">The device was updated.</response>
    /// <response code="400">The request is invalid.</response>
    /// <response code="404">The device was not found.</response>
    /// <response code="409">The update violates a domain rule.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpPatch("{id}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Patch(
        [FromRoute] Guid id,
        [FromBody] PatchDeviceRequest request,
        CancellationToken cancellationToken)
    {
        var result = await patchHandler.HandleAsync(
            new PatchDeviceCommand(id, request.Name, request.Brand, request.State),
            cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error!.ToProblem();
    }

    /// <summary>
    /// Deletes a device.
    /// </summary>
    /// <param name="id">Device identifier.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <response code="204">The device was deleted.</response>
    /// <response code="400">The identifier is invalid.</response>
    /// <response code="404">The device was not found.</response>
    /// <response code="409">The device cannot be deleted while it is in use.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await deleteHandler.HandleAsync(
            new DeleteDeviceCommand(id),
            cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.Error!.ToProblem();
    }
}
