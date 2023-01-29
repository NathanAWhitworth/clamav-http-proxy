namespace ClamAVHttpProxy.Controllers;

using Microsoft.AspNetCore.Mvc;
using nClam;

[ApiController]
[Route("[controller]")]
public class ScanController : ControllerBase
{
    private readonly ILogger<ScanController> _logger;
    private readonly ClamClient _clamav;

    public ScanController(ILogger<ScanController> logger, ClamClient clamav)
    {
        _logger = logger;
        _clamav = clamav;
    }

    [HttpPost("raw")]
    public async Task<IActionResult> ScanRaw()
    {
        if (HttpContext.Request.ContentLength == 0)
        {
            _logger.LogWarning("Request body was empty.");
            return StatusCode(400);
        }

        var scanResult = await _clamav.SendAndScanFileAsync(HttpContext.Request.Body);

        switch (scanResult?.Result)
        {
            case ClamScanResults.Clean:
                _logger.LogDebug("ClamAV reported file is clean.");
                return Ok(0);

            case ClamScanResults.VirusDetected:
                if (scanResult?.InfectedFiles is not null)
                {
                    foreach (var detection in scanResult.InfectedFiles)
                    {
                        _logger.LogInformation("ClamAV detected {VirusName} in file.", detection.VirusName);
                        HttpContext.Response.Headers.Add("X-Detected", detection.VirusName);
                    }
                }

                return Ok(1);

            default:
                _logger.LogError("Unexpected response from ClamAV.");
                return StatusCode(500);
        }
    }
}
