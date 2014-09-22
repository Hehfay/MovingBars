using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {

  private AnimationClip clip;

  private bool do_play_animation = false;

  private bool is_first_time = true;

  // Use this for initialization
  void Start () {

if( false)
{
    // This mechanism turns out to be unnecessary. The curves can be set from
    // elsewhere. One thing that does need to be in this script is event
    // functions.
    
    if( is_first_time)
    {
      /*AnimationClip*/ clip = new AnimationClip();
      clip.name = "move_bar";

      AnimationCurve curve;
      float dt = (
                   Mathf.Abs( transform.localPosition.x
                            - ( Main.i0+(Main.i-1)*Main.di)
                            )
                 )
                 /
                 (
                   Main.ni*Main.di
                 );

      curve = AnimationCurve.Linear( 0, transform.localPosition.x
                                   , dt, Main.i0+(Main.i-1)*Main.di);
      clip.SetCurve("", typeof(Transform), "localPosition.x", curve);

      curve = AnimationCurve.Linear( 0, transform.localPosition.y
                                   , dt, transform.localPosition.y);
      clip.SetCurve("", typeof(Transform), "localPosition.y", curve);

      curve = AnimationCurve.Linear( 0, transform.localPosition.z
                                   , dt, transform.localPosition.z);
      clip.SetCurve("", typeof(Transform), "localPosition.z", curve);

      AnimationEvent evt = new AnimationEvent();
      evt.time = dt;
      evt.functionName = "eventDoneMovingBarj";
      clip.AddEvent(evt);

      animation.AddClip(clip,clip.name);
      animation.clip = clip;
      //animation.Play(clip.name);

  //  AnimationCurve curve;
  //  curve = AnimationCurve.Linear( 0, transform.localPosition.y
  //                               , 1, transform.localPosition.y+1);
  //  /* AnimationClip */ clip = new AnimationClip();
  //  clip.name = "test";
  //
  //  clip.SetCurve("", typeof(Transform), "localPosition.y", curve);
  //  curve = AnimationCurve.Linear( 0, transform.localPosition.x
  //                               , 1, transform.localPosition.x);
  //  clip.SetCurve("", typeof(Transform), "localPosition.x", curve);
  //  curve = AnimationCurve.Linear( 0, transform.localPosition.z
  //                               , 1, transform.localPosition.z);
  //  clip.SetCurve("", typeof(Transform), "localPosition.z", curve);
  //
  //  AnimationEvent evt = new AnimationEvent();
  //  evt.time = 1;
  //  evt.functionName = "eventEndOfClip";
  //  clip.AddEvent(evt);
  //
  //  animation.AddClip(clip, clip.name);
  //  animation.Play(clip.name);
  //
  ////AssetDatabase.ImportAsset("Assets/SpinAnim");
  ////AnimationClip clip = Resources.Load<AnimationClip>("Assets/SpinAnim.anim");
  //
  //  //clip = (AnimationClip)Resources.Load("SpinAnim", typeof(AnimationClip));
  //
  //  if( clip==null)
  //  {
  //    print("clip is null");
  //  }
  //  else
  //  {
  //    print("clip is not null: " + clip.name);
  //  }
  //
  //  animation.AddClip( clip, clip.name);
  //  animation.clip = clip;
    }
}
  }
  
  // Update is called once per frame
  void Update () {

if( false)
{

    if( is_first_time)
    {
      if( Main.is_moving_bar_j)
      {
        //print("bar is moving..?");
        if( !animation.IsPlaying(clip.name))
        {
          print("playing clip "+clip.name+"..?");
          animation.Play(clip.name);
        }
      }
    }

    is_first_time = false;


//  if(do_play_animation)
//  {
//    animation.Play(clip.name);
//    do_play_animation = false;
//  }

}
  }

//void eventEndOfClip()
//{
//  print("end of animation clip");
//}

  void eventDoneMovingBari()
  {
    Main.is_moving_bar_i = false;
    if( Main.i<Main.ni-1)
    {
      renderer.material.color = new Color(0.9f,0.4f,0.2f);
    }
    else
    {
      renderer.material.color = new Color(0.5f,0.7f,1.0f);
    }
    transform.localPosition = new Vector3( Main.i0 + Main.j*Main.di*Main.kernfac, Main.j0, Main.di);
    print("done moving bari");
  }

  void eventDoneMovingBarj()
  {
    Main.is_moving_bar_j = false;
    renderer.material.color = new Color(0.5f,0.7f,1.0f);
    transform.localPosition = new Vector3( Main.i0 + Main.i*Main.di*Main.kernfac, Main.j0, Main.di);
    print("done moving barj");
  }

  void eventDoneMovingBars()
  {
    Main.is_moving_bars = false;
    print("done moving bars");
  }
}
