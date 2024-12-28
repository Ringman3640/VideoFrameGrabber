namespace VideoFrameGrabber
{
    /// <summary>
    /// Specifies the order of scaling and cropping.
    /// </summary>
    public enum ScaleCropOrder
    {
        /// <summary>
        /// Indicates scaling should be applied before cropping.
        /// </summary>
        ScaleFirst,

        /// <summary>
        /// Indicates cropping should be applied before scaling. 
        /// </summary>
        CropFirst
    }
}
