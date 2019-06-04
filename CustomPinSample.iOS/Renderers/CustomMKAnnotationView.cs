using System;
using MapKit;

namespace CustomPinSample.iOS.Renderers
{
    internal class CustomMKAnnotationView : MKAnnotationView
    {


        public CustomMKAnnotationView(IMKAnnotation annotation, string reuseIdentifier) : base(annotation, reuseIdentifier)
        {
        }
    }
}
