using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using System.Collections.Generic;
using UIKit;
using MapKit;
using CoreGraphics;
using Xamarin.Forms.Maps;
using CustomPinSample.iOS.Renderers;
using CustomPinSample.Views;
using CustomPinSample.Controls;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace CustomPinSample.iOS.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        UIView customPinView;
        Map formsMap;
        IList<Pin> pins;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                var nativeMap = Control as MKMapView;
                if (nativeMap != null)
                {
                    nativeMap.RemoveAnnotations(nativeMap.Annotations);
                    nativeMap.GetViewForAnnotation = null;
                    nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
                    nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
                    nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
                }
            }

            if (e.NewElement != null)
            {
                formsMap = (CustomMap)e.NewElement;
                var nativeMap = Control as MKMapView;
                pins = new List<Pin>();
                pins = formsMap.Pins;

                nativeMap.GetViewForAnnotation = GetViewForAnnotation;
                nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
                nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
                nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
            }
        }



        protected override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            if (annotation is MKUserLocation)
                return null;

            var anno = annotation as MKPointAnnotation;
            if (anno == null)
                return null;

            var pin = GetPin(anno);
            if (pin == null)
            {

                throw new Exception("pin not found");
            }

            annotationView = mapView.DequeueReusableAnnotation(pin.MarkerId.ToString());
            if (annotationView == null)
            {
                annotationView = new CustomMKAnnotationView(annotation, pin.MarkerId.ToString());
                annotationView.Image = UIImage.FromFile("pin.png");
                annotationView.CalloutOffset = new CGPoint(0, 0);

            }
            annotationView.CanShowCallout = true;

            return annotationView;
        }

        void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
        {

        }

        void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            var customView = e.View as CustomMKAnnotationView;
            customPinView = new UIView();

        }

        void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            if (!e.View.Selected)
            {
                customPinView.RemoveFromSuperview();
                customPinView.Dispose();
                customPinView = null;
            }
        }

        Pin GetPin(MKPointAnnotation annotation)
        {

            var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);


            foreach (var pin in pins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }


            return null;
        }
    }
}