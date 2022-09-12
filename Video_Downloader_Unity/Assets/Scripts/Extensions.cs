using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YoutubeExplode.Videos.ClosedCaptions;
using YoutubeExplode.Videos.Streams;

public static class Extensions
{
    public static MuxedStreamInfo WithHighestVideoQualitySupported(this IEnumerable<MuxedStreamInfo> streamInfos)
    {
        if(streamInfos == null)
            throw new ArgumentNullException(nameof(streamInfos));
        return streamInfos
            .Where(info => info.Container == Container.Mp4)
            .Select(info => info).OrderByDescending(s => s.VideoQuality).FirstOrDefault();
    }
    public static MuxedStreamInfo WithHighestVideoQualitySupported(this StreamManifest streamManifest)
    {
        if(streamManifest == null)
            throw new ArgumentNullException(nameof(streamManifest));
        return streamManifest.GetMuxed().WithHighestVideoQualitySupported();
    }
}
