//using UnityEditor;
//using UnityEngine;


//[CustomEditor(typeof(PingPong))]

//public class PingPongEditor : Editor
//{

//    public  float playoneMove,xVal,yVal,testVal;
//    public Color bttnClr,playHitBtnClr;
//    PingPong ppRef;

    

//    public override void OnInspectorGUI()
//    {
    
//        ppRef = target as PingPong;
//        playoneMove = ppRef.playerPostion;

//        base.OnInspectorGUI();

       

//        ppRef.buttnWidth = ((EditorGUIUtility.currentViewWidth / 2f) - (EditorGUIUtility.currentViewWidth / 5f) / 2f);
//        xVal = ppRef.curX;
//        yVal = ppRef.curY;
//        bttnClr = ppRef.hitColor;
//        playHitBtnClr = ppRef.playerHitColor;
//        ppRef.vaL = EditorGUIUtility.currentViewWidth;
//        ppRef.inspectorWidth = Mathf.Round(((EditorGUIUtility.currentViewWidth / 2f) - (EditorGUIUtility.currentViewWidth / 5f) / 2f) + playoneMove);


//        //GUILayout.BeginHorizontal();

//        // GUILayout.Space(((EditorGUIUtility.currentViewWidth / 2f) - (EditorGUIUtility.currentViewWidth / 5f) / 2f) + playoneMove);

//        if (!ppRef.isTimer && !ppRef.isStarted)
//        {
//            EditorGUILayout.BeginHorizontal();
//            GUILayout.Space(EditorGUIUtility.currentViewWidth / 2f - (EditorGUIUtility.currentViewWidth / 5f) / 2f);
//            if (GUILayout.Button("Start", GUILayout.Width(EditorGUIUtility.currentViewWidth / 5f)))
//            {
//                ppRef.isTimer = true;
//                Debug.Log("Pressed---" + ppRef.isTimer);
//            }
//            EditorGUILayout.EndHorizontal();
//        }

       


//        GUILayout.Space(80f);

//        // GUILayout.BeginHorizontal();
//        //  EditorGUILayout.Space(playoneMove);

//        if (ppRef.isStarted)
//        {

//            EditorGUILayout.BeginHorizontal();


//            GUILayout.Space(playoneMove);
//            EditorGUI.DrawRect(new Rect(0f, 200f, EditorGUIUtility.currentViewWidth, 300f), Color.black);
//            //  GUILayout.Toggle(false, GUIContent.none, GUILayout.Width(50f));

//            //Color originalBackgroundColor1 = GUI.backgroundColor;
//            //GUI.backgroundColor = Color.green;

//            // EditorGUI.DrawRect(new Rect(0f,0f, EditorGUIUtility.currentViewWidth, 300f), Color.grey);
//        }

//        if (ppRef.isTimer)
//        {

//            GUIStyle yellowBackgroundStyleT = new GUIStyle(GUI.skin.textArea);
//            yellowBackgroundStyleT.alignment = TextAnchor.MiddleCenter;
//            yellowBackgroundStyleT.fontSize = 500;
//            GUILayout.BeginHorizontal();
//            GUILayout.Space(-20f);
//            EditorGUILayout.TextArea(ppRef.timeText, yellowBackgroundStyleT, GUILayout.Width(EditorGUIUtility.currentViewWidth), GUILayout.Height(400));
//            GUILayout.EndHorizontal();
//        }





//        if (ppRef.isStarted)
//        {




//            GUIStyle yellowBackgroundStyle = new GUIStyle(GUI.skin.button);
//            yellowBackgroundStyle.normal.background = MakeBackgroundTexture(1, 1, playHitBtnClr);
//            yellowBackgroundStyle.fontStyle = FontStyle.Bold;
//            yellowBackgroundStyle.normal.textColor = Color.white;

//            if (GUILayout.Button("Player", yellowBackgroundStyle, GUILayout.Width(EditorGUIUtility.currentViewWidth / 5f)))
//            {
//                Debug.Log("Pressed-" + EditorGUIUtility.currentViewWidth);
//            }

//            // GUI.backgroundColor = originalBackgroundColor1;



//            //if (GUI.Button(new Rect(playoneMove,150f,100f,20f),"Player--1"))
//            //{
//            //    Debug.Log("Pressed-" + EditorGUIUtility.currentViewWidth);
//            //}


//            EditorGUILayout.EndHorizontal();

//            //  GUILayout.EndHorizontal();


//            // GUILayout.Space(500f);//needed




//            //GUIStyle yellowBackgroundStyle2 = new GUIStyle(GUI.skin.button);
//            //yellowBackgroundStyle2.normal.background = MakeBackgroundTexture(1, 1, Color.red);
//            //yellowBackgroundStyle2.fontStyle = FontStyle.Bold;
//            //yellowBackgroundStyle2.normal.textColor = Color.white;
//            //if (GUILayout.Button("", yellowBackgroundStyle2, GUILayout.Width(2),GUILayout.Height(10)))
//            //{
//            //    Debug.Log("Pressed" + EditorGUIUtility.currentViewWidth);
//            //}

//            //EditorGUILayout.TextField("Obj", GUILayout.Height(400));



//            // dotv2(xVal,yVal,50f,50f);

//            dotv3(xVal, yVal, 50f, 50f);



//            GUILayout.Space(200f);//needed


//            GUILayout.BeginHorizontal();
//            float val = Mathf.Clamp(xVal, 0f, 318f);
//            GUILayout.Space(val - 20f);


//            //GUILayout.Space(((EditorGUIUtility.currentViewWidth / 2f) - (EditorGUIUtility.currentViewWidth / 5f) / 2f));

//            // Color originalBackgroundColor = GUI.backgroundColor;
//            // GUI.backgroundColor = Color.red;


//            GUIStyle yellowBackgroundStyle1 = new GUIStyle(GUI.skin.button);
//            yellowBackgroundStyle1.normal.background = MakeBackgroundTexture(1, 1, bttnClr);
//            yellowBackgroundStyle1.fontStyle = FontStyle.Bold;
//            yellowBackgroundStyle1.normal.textColor = Color.white;


//            if (GUILayout.Button("AI", yellowBackgroundStyle1, GUILayout.Width(EditorGUIUtility.currentViewWidth / 5f)))
//            {
//                Debug.Log("Pressed" + EditorGUIUtility.currentViewWidth);
//            }

//            // GUI.backgroundColor = originalBackgroundColor;

//            GUILayout.EndHorizontal();


//            //GUILayout.Space(50f);

//            //GUILayout.BeginHorizontal();
//            //GUILayout.Space(((EditorGUIUtility.currentViewWidth / 2f) - (EditorGUIUtility.currentViewWidth / 5f) / 2f));

//            //if (GUILayout.RepeatButton(">>", GUILayout.Width(EditorGUIUtility.currentViewWidth / 10f)))
//            //{
//            //    testVal += 1f;
//            //    Debug.Log("Pressed" + EditorGUIUtility.currentViewWidth + "--==--" + testVal);
//            //}


//            //if (GUILayout.RepeatButton("<<", GUILayout.Width(EditorGUIUtility.currentViewWidth / 10f)))
//            //{
//            //    Debug.Log("Pressed" + EditorGUIUtility.currentViewWidth);
//            //}
//            //GUILayout.EndHorizontal();






//            //EditorGUILayout.LabelField("[[][][][][][][][][][][]]");
//            GUILayout.Space(playoneMove + 600f);

//            //if (GUI.changed)
//            //{
//            //    Debug.Log("Pressed");

//            //}
//        }

//    }

//    void OnInspectorUpdate()
//    {
//        Debug.Log("Pressed when updated");
        
//    }

 

//    public void dot(GUILayout curLayout,float width,float height)
//    {
//        GUILayout.BeginHorizontal();
//        GUILayout.Space(50f);
//        GUILayout.Toggle(false, GUIContent.none, GUILayout.Width(50f));

//        //GUILayout.Button("O", GUILayout.Width(EditorGUIUtility.currentViewWidth / 5f));
//        GUILayout.EndHorizontal();
//    }


//    public void dotv2(float width,float height,float maxHeight,float maxWidth)
//    {
//        GUILayout.BeginArea(new Rect(width,height, maxWidth, maxHeight));//230f
//        Color originalBackgroundColor = GUI.backgroundColor;
//        GUI.backgroundColor = Color.white;
//        GUIStyle yellowBackgroundStyle = new GUIStyle(GUI.skin.toggle);       
//        GUILayout.Toggle(false, GUIContent.none,yellowBackgroundStyle, GUILayout.Width(50f));

//        GUI.backgroundColor = originalBackgroundColor;


//        GUILayout.EndArea();
//    }


//    public void dotv3(float width, float height, float maxHeight, float maxWidth)
//    {
//        GUILayout.BeginArea(new Rect(width, height, maxWidth, maxHeight));//230f
//        //Color originalBackgroundColor = GUI.backgroundColor;
//        //GUI.backgroundColor = Color.white;
//        //GUIStyle yellowBackgroundStyle = new GUIStyle(GUI.skin.toggle);
//        //GUILayout.Toggle(false, GUIContent.none, yellowBackgroundStyle, GUILayout.Width(50f));

//        //GUI.backgroundColor = originalBackgroundColor;





//        GUIStyle yellowBackgroundStyle2 = new GUIStyle(GUI.skin.button);
//        yellowBackgroundStyle2.normal.background = MakeBackgroundTexture(1, 1, Color.white);
//        yellowBackgroundStyle2.fontStyle = FontStyle.Bold;
//        yellowBackgroundStyle2.normal.textColor = Color.white;


//        if (GUILayout.Button("", yellowBackgroundStyle2, GUILayout.Width(12), GUILayout.Height(12)))
//        {
//            Debug.Log("Pressed" + EditorGUIUtility.currentViewWidth);
//        }
//        GUILayout.EndArea();
//    }


//    private Texture2D MakeBackgroundTexture(int width, int height, Color color)
//    {
//        Color[] pixels = new Color[width * height];

//        for (int i = 0; i < pixels.Length; i++)
//        {
//            pixels[i] = color;
//        }

//        Texture2D backgroundTexture = new Texture2D(width, height);

//        backgroundTexture.SetPixels(pixels);
//        backgroundTexture.Apply();

//        return backgroundTexture;
//    }

//}
