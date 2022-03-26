using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;
public class MeasureDepth : MonoBehaviour
{
private Texture2D newTexture;
private Color Area_1=Color.red;
private Color Area_2=Color.green;
private Color Area_3=Color.black;

public Vector2Int distance_area_1;
public Vector2Int distance_area_2;
public RawImage DepthImage;

//update the image at every frame
void Update(){
    DepthImage.texture=newTexture;
    }
private void Awake(){
    newTexture=new Texture2D(512,424);
    for(int i=0;i<512;i++){
        for(int j=0;j<424;j++){
            newTexture.SetPixel(i,j,Color.black);
            }
        }
    newTexture.Apply();
    }

//recive array of points and apply color to the relative frame depends on the point's distance
public void UpdateTexture(Vector3[] points){
    Color new_color;
    foreach(Vector3 point in points){
         //distance=Length(point);
         new_color=Distance_to_color(point.z);
        newTexture.SetPixel((int)point.x,(int)point.y,new_color);
        }
    newTexture.Apply();
}

//recive points distance and return its area's color
private Color Distance_to_color(double distance){
    if((distance>=distance_area_1[0])&&(distance<distance_area_1[1])){
            return Area_1;
        }
    else if((distance>=distance_area_2[0])&&(distance<=distance_area_2[1])){
            return Area_2;
        }
    return Area_3;
}
//option to calculate the distance from the center of the sensor.
public double Length(Vector3 point)
{
	return Mathf.Sqrt(
		point.x * point.x +
		point.y * point.y +
		point.z * point.z
	);
}

}
