namespace VideoFrameGrabber.Cropping
{
    /// <summary>
    /// Specifies the position aligmnent of a crop rectangle. 
    /// </summary>
    /// <remarks>
    /// This position value indicates how a cropping object should position the crop rectangle.
    /// </remarks>
    public enum CropAlign
    {
        /// <summary>
        /// Provide no alignment value for a crop operation.
        /// </summary>
        /// <remarks>
        /// This value indicates that a cropping object should either not perform any special
        /// offset positioning (set offset values to 0), or that the object should use some other
        /// available value that is not a <see cref="CropAlign"/> value (such as direct offset
        /// values).
        /// </remarks>
        None,

        /// <summary>
        /// Position the crop rectangle against the top left corner of the original image.
        /// </summary>
        TopLeft,

        /// <summary>
        /// Position the crop rectangle against the top center area of the original image.
        /// </summary>
        TopCenter,

        /// <summary>
        /// Position the crop rectangle against the top right corner of the original image.
        /// </summary>
        TopRight,

        /// <summary>
        /// Position the crop rectangle against the left center area of the original image.
        /// </summary>
        CenterLeft,

        /// <summary>
        /// Position the crop rectangle at the center of the original image.
        /// </summary>
        Center,

        /// <summary>
        /// Position the crop rectangle against the right center area of the original image.
        /// </summary>
        CenterRight,

        /// <summary>
        /// Position the crop rectangle against the bottom left corner of the original image.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// Position the crop rectangle against the bottom center area of the original image.
        /// </summary>
        BottomCenter,

        /// <summary>
        /// Position the crop rectangle against the bottom right corner of the original image.
        /// </summary>
        BottomRight,
    }
}
