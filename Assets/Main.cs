using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

  private static int[] bars;

  private GameObject bari;
  private GameObject barj;

  private GameObject go;

  public static int ni = 64;
  public static int i = 0;
  public static int j = 0;
  private static int temp = 0;
  public static float di = 0.1f;
  public static float kernfac = 1.5f;
  public static float i0 = -(ni/2+1)*di*kernfac;
  public static float j0 = -(ni/2)*di;

  private static bool do_init_phase = true;
  private static bool do_move_phase = false;
  private static bool do_loop = false;
  private static bool do_reinit_phase = false;
  private static bool do_automatically_reinit = false;

  public static bool is_moving_bar_i = false;
  public static bool is_moving_bar_j = false;
  public static bool is_moving_bars  = false;

  // 84, 74, 61

  // Use this for initialization
  void Start () {

    go = new GameObject("Bar0000");

    Vector3[] Vertices;
    Vector2[] UV;
    int[] Triangles;

    Vertices = new Vector3[] // cube
    {
      new Vector3(0,0,0) // 0
    , new Vector3(0,1,0) // 1
    , new Vector3(1,1,0) // 2
    , new Vector3(1,0,0) // 3
    , new Vector3(0,0,1) // 4
    , new Vector3(0,1,1) // 5
    , new Vector3(1,1,1) // 6
    , new Vector3(1,0,1) // 7
    };

    UV = new Vector2[] // ???
    {
      new Vector2(0,0) // 0
    , new Vector2(0,1) // 1
    , new Vector2(1,1) // 2
    , new Vector2(1,0) // 3
    , new Vector2(0,0) // 4
    , new Vector2(0,1) // 5
    , new Vector2(1,1) // 6
    , new Vector2(1,0) // 7
    };

    //          y
    //          |
    //   z      |
    //    \   5-|-----6
    //     \  |\|     |\
    //      \ | 1-------2
    //       \| |     | |
    //        4-|-----7 |
    //         \|      \|
    //          0-------3------- x
    //

    Triangles = new int[] // two triangles per side (six sides)
    {
      0, 1, 2 , 0, 2, 3   // front
    , 1, 0, 4 , 1, 4, 5   // left
    , 4, 6, 5 , 4, 7, 6   // back
    , 6, 7, 3 , 6, 3, 2   // right
    , 1, 6, 2 , 1, 5, 6   // top
    , 0, 3, 7 , 0, 7, 4   // bottom
    };

    go.AddComponent<MeshFilter>();

    Mesh mesh = go.GetComponent<MeshFilter>().mesh;

    mesh.vertices = Vertices;
    mesh.uv = UV;
    mesh.triangles = Triangles;

    mesh.RecalculateNormals();

    go.AddComponent<MeshRenderer>();
    go.renderer.enabled = false;
    //go.renderer.material.color = Color.red;
    go.renderer.material.color = new Color( 1.0f, 0.5f, 0.3f);

    go.transform.localScale = new Vector3( di,1.0f,di); // squish it
    go.transform.localPosition += new Vector3(-0.05f,0.0f,-0.05f); // center it

    GameObject light01 = new GameObject("light01");
    light01.AddComponent<Light>();
    light01.light.color = Color.white;
    light01.transform.localPosition = new Vector3( 0, 2.5f, -5);
    light01.light.range = 16;

    GameObject light02 = new GameObject("light02");
    light02.AddComponent<Light>();
    light02.light.color = Color.white;
    light02.transform.localPosition = new Vector3( -5, 0, -5);
    light02.light.range = 16;

    GameObject light03 = new GameObject("light03");
    light03.AddComponent<Light>();
    light03.light.color = Color.white;
    light03.transform.localPosition = new Vector3(  5, 0, -5);
    light03.light.range = 16;

    //--------------------------------------------------------------------------

    InitBars();

  }

  // Update is called once per frame
  void Update () {

    if( do_init_phase) { InitPhase();}

    if( !( is_moving_bar_i || is_moving_bar_j) && do_move_phase)
    {
      MovePhase();
    }

    if( !is_moving_bars && do_reinit_phase) { ReInitPhase();}

  }

//##############################################################################

  void InitBars() {

    bars = new int[ni];

    int i;
    for( i=0; i<ni; i++)
    {
      bars[i] = i+1;
    }

    if( /* Shuffle */ false)
    {
      int j;
      int temp;
      for( i=ni-1; i>0; i--)
      {
        j = Random.Range(0,i-1);
        temp = bars[j];
        bars[j] = bars[i];
        bars[i] = temp;
      }
    }
  }

  void InitPhase() {

    if( i<ni)
    {
      var x = (GameObject)Instantiate(go);
      x.name = "Bar"+string.Format("{0:0000}",bars[i]);
      x.renderer.enabled = true;
      x.transform.localPosition = new Vector3( i0 + i*di*kernfac, j0, 0);
      x.transform.localScale = new Vector3( di, bars[i]*di, di);
      x.AddComponent<Animation>();
      x.AddComponent("Spin");
      i++;
    }
    else
    {
      do_init_phase = false;
      do_move_phase = true;
      i = 0;
    }
  }

  void ReInitPhase() {

    if( i<ni)
    {
      //var x = (GameObject)Instantiate(go);
      //x.name = "Bar"+string.Format("{0:0000}",bars[i]);
      var x = GameObject.Find("Bar"+string.Format("{0:0000}",i+1));
      x.transform.localPosition = new Vector3( i0 + i*di*kernfac, j0, 0);
      x.transform.localScale = new Vector3( di, (i+1)*di, di);
      //x.renderer.material.color = Color.red;
      x.renderer.material.color = new Color( 1.0f, 0.5f, 0.3f);
      i++;
    }
    else
    {
      do_reinit_phase = false;
      do_move_phase = true;
      for( i=0; i<ni; i++)
      {
        bars[i] = i+1;
      }
      i = 0;
    }
  }

  void MovePhase() {

    if( i<ni-1)
    {
      bari = GameObject.Find("Bar"+string.Format("{0:0000}",bars[i]));
      bari.renderer.material.color = Color.white;

      //j = i; while( j==i) { j = Random.Range(0,ni-1);}
      j = Random.Range(i+1,ni);

      barj = GameObject.Find("Bar"+string.Format("{0:0000}",bars[j]));
      barj.renderer.material.color = Color.white;

      temp = bars[j];
      bars[j] = bars[i];
      bars[i] = temp;

      float dt;
      AnimationCurve curve;
      AnimationClip clip_i = new AnimationClip();
      AnimationEvent evt_i = new AnimationEvent();

      //bari.transform.localPosition = new Vector3( i0 + j*di, j0, 0);
      is_moving_bar_i = true;
      print(bari.name + " is moving");

      if( /* animate bari */ true)
      {
        clip_i.name = "move_bar_i";

        dt = (
               Mathf.Abs( bari.transform.localPosition.x
                        - ( i0+j*di)
                        )
             )
             /
             (
               ni*di
             );
        dt = 0.5f*dt;

        curve = AnimationCurve.EaseInOut( dt/2.0f
                                        , bari.transform.localPosition.x
                                        , dt, i0+j*di*kernfac);
        clip_i.SetCurve("", typeof(Transform), "localPosition.x", curve);
        // Note: AnimationClip.SetCurve replaces previous curves.

        curve = AnimationCurve.Linear( 0, j0, dt, j0);
        clip_i.SetCurve("", typeof(Transform), "localPosition.y", curve);

        curve = AnimationCurve.Linear( 0, -di, dt, 0);
        clip_i.SetCurve("", typeof(Transform), "localPosition.z", curve);

        evt_i.time = dt;
        evt_i.functionName = "eventDoneMovingBari";
        clip_i.AddEvent(evt_i);

        bari.animation.AddClip(clip_i,clip_i.name); // replaces
      }
      else
      {
        bari.transform.localPosition = new Vector3( i0 + j*di*kernfac, j0, 0);
        is_moving_bar_i = false;
      }

      AnimationClip clip_j = new AnimationClip();
      AnimationEvent evt_j = new AnimationEvent();

      //barj.transform.localPosition = new Vector3( i0 + i*di, j0, 0);
      is_moving_bar_j = true;
      print(barj.name + " is moving");

if( true)
{
  if( /* animate barj */ true)
  {
        clip_j.name = "move_bar_j";

      //dt = (
      //       Mathf.Abs( barj.transform.localPosition.x
      //                - ( i0+i*di)
      //                )
      //     )
      //     /
      //     (
      //       ni*di
      //     );
      //dt = 2.5f*dt;

        curve = AnimationCurve.EaseInOut( dt/2.0f
                                        , barj.transform.localPosition.x
                                        , dt, i0+i*di*kernfac);
        clip_j.SetCurve("", typeof(Transform), "localPosition.x", curve);
        // Note: AnimationClip.SetCurve replaces previous curves.

        curve = AnimationCurve.Linear( 0, j0, dt, j0);
        clip_j.SetCurve("", typeof(Transform), "localPosition.y", curve);

        curve = AnimationCurve.Linear( 0, -di, dt, 0);
        clip_j.SetCurve("", typeof(Transform), "localPosition.z", curve);

        evt_j.time = dt;
        evt_j.functionName = "eventDoneMovingBarj";
        clip_j.AddEvent(evt_j);

        barj.animation.AddClip(clip_j,clip_j.name); // replaces

        bari.animation.Play(clip_i.name);
        barj.animation.Play(clip_j.name);
      //bari.transform.localPosition = new Vector3( i0 + j*di*kernfac, j0, 0);
      //barj.transform.localPosition = new Vector3( i0 + i*di*kernfac, j0, 0);
  }
  else
  {
      barj.transform.localPosition = new Vector3( i0 + i*di*kernfac, j0, 0);
      is_moving_bar_j = false;
  }
}
else
{
      // This way is unecessary. See above. The only thing that needs to be
      // scripted on the object itself is the event function.

      if(GetComponent<Spin>())
      {
        var scripts = GetComponents<Spin>();
        foreach( Spin script in scripts)
        {
          print("destroying dupe script");
          Destroy(script);
        }
      }
      barj.AddComponent("Spin");

      //print("Script Type: " + System.Type.GetType("Spin"));
}


      i++;
    }
    else
    {
      if( do_loop)
      {
        i = 0;
      }
      else
      {
        do_move_phase = false;
        //barj = GameObject.Find("Bar"+string.Format("{0:0000}",bars[ni-1]));
        //barj.AddComponent("Spin");
        //barj.AddComponent<Animation>();

        if( do_automatically_reinit)
        {
          do_reinit_phase = true;
          i = 0;
        }
      }
    }
  }

  void Restart() {

    do_move_phase = false;
    is_moving_bar_i = false;
    is_moving_bar_j = false;

    if( true)
    {
      is_moving_bars = true;

      float dt;
      AnimationCurve curve;

      int ii;
      float r;

      for( ii=0; ii<ni; ii++)
      {
        var bari = GameObject.Find("Bar"+string.Format("{0:0000}",ii+1));

        AnimationClip clip = new AnimationClip();
        AnimationEvent evt = new AnimationEvent();

        if( /* animate bari */ true)
        {
          clip.name = "move_bar_"+(ii+1);

          dt = 1.0f;

          r = Random.Range(0,dt)/2;

          curve = AnimationCurve.Linear( r, bari.transform.localPosition.x
                                       , dt, bari.transform.localPosition.x);
          clip.SetCurve("", typeof(Transform), "localPosition.x", curve);
          // Note: AnimationClip.SetCurve replaces previous curves.

          curve = AnimationCurve.EaseInOut( r, bari.transform.localPosition.y
                                          , dt, -15);
          clip.SetCurve("", typeof(Transform), "localPosition.y", curve);

          curve = AnimationCurve.Linear( r, bari.transform.localPosition.z
                                       , dt, bari.transform.localPosition.z);
          clip.SetCurve("", typeof(Transform), "localPosition.z", curve);

          evt.time = dt;
          evt.functionName = "eventDoneMovingBars";
          clip.AddEvent(evt);

          bari.animation.AddClip(clip,clip.name); // replaces
          bari.animation.Play(clip.name);
        }
      }
    }

    do_reinit_phase = true;
    i = 0;
  }

void OnGUI() {

  if( GUI.Button( new Rect( Screen.width/2-Screen.width/20
                          , Screen.height-10-Screen.height/15
                          , Screen.width/10
                          , Screen.height/15 ) , "Restart"))
  {
    Restart();
  }

}

}
