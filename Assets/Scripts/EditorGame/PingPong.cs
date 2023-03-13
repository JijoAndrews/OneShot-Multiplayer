//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

    
//public class PingPong : MonoBehaviour
//{


//    //Start is called before the first frame update

//    [HideInInspector]
//    public Vector2 ballPos, playerPos;

//    public float  inspectorWidth,playerPostion;

//   // [HideInInspector]
//    public float curX, curY;

//   [HideInInspector]
//    public float vaL, yVal, hitx, hitY, dotX;

//    [HideInInspector]
//    public float windowWidth,maxLeftWidth,dotMaxWdith,dotMaxHeight,buttnWidth,dotY, dotXrev,oneVal,aiMovVal;

//    //[HideInInspector]
//    //public bool isMoving,isMovingLeft,isMovingRight;

//    [HideInInspector]
//    public static float moveVal;

//    [HideInInspector]
//    public bool isMoving,isReachedMaxX, isMovingLeft, isReached, isReachedMaxY;

//    [HideInInspector]
//    public bool gotOut,gotHit;

//    [HideInInspector]
//    public bool isMovingRight,isTimer=false,isStarted=false;

//    [HideInInspector]
//    public Color hitColor=Color.red,playerHitColor=Color.red, ballColour;

//    [HideInInspector]
//    public string timeText;

//    [HideInInspector]
//    public float  secs, milliSecs;

//    public float changeX, changeY;
//    void Start()
//    {
//        hitColor = Color.red;
//        timeText = "3";
//      dotX = vaL - 20f;
//        dotY = 485f;

//        curX = dotX/2f;
//        curY =430f;

//        changeY = -1f;

//        Debug.Log(dotX);
//        hitx = Mathf.Round(Random.Range(0f, dotX));
//        hitY = Mathf.Round(Random.Range(225f, dotY));
//        isMoving = true;
//        isTimer = false;

//        // ballMov();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (secs <= 3f && isTimer)
//        {
//            milliSecs += 0.005f;

//            if (milliSecs>= 1f)
//            {
//                milliSecs = 0f;
//                secs += 1f;
//                timeText = (3f - secs).ToString();
//            }


//        }

//        if (secs>=3f)
//        {
//            isStarted = true;
//            isTimer = false;
//            secs = 0f;
//        }


//        ballPos = new Vector2(curX, curY);
//        playerPos = new Vector2(playerPostion,80f);
        
//        //if (Input.GetKey(KeyCode.A) && tempVal<200f)
//        //{
//        //    moveVal += 1f;
//        //    tempVal += 1f;
//        //    isMoving = true;


//        //}

//        //if (Input.GetKey(KeyCode.D) &&  tempVal >-200f)
//        //{
//        //    moveVal -= 1f;
//        //    tempVal -= 1f;
//        //    // Debug.Log("movVal-" + moveVal);
//        //    isMoving = true;

//        //}

//        dotXrev = -1f * dotX;


//        //if (Input.GetKey(KeyCode.A) && !isMovingRight && tempVal > dotXrev)
//        //{


//        //    moveVal -= 1f;
//        //    tempVal -= 1f;

//        //    isMovingLeft = true;
//        //}
//        //else
//        //{
//        //    isMovingLeft = false;
//        //}

//        //if (Input.GetKey(KeyCode.D) && !isMovingLeft && tempVal < dotX)
//        //{
//        //    moveVal += 1f;
//        //    tempVal += 1f;

//        //    isMovingRight = true;
//        //}
//        //else
//        //{
//        //    isMovingRight = false;
//        //}


//       // MovPlayer1();

//        if (Input.GetKey(KeyCode.A) && !isMovingRight && playerPostion>0f)
//        {
//            playerPostion -= 2f;
//            isMovingLeft = true;
//           // Debug.Log("movval=" + tempVal);

//        }
//        else
//        {
//            isMovingLeft = false;
//        }

//        if (Input.GetKey(KeyCode.D) && !isMovingLeft && playerPostion<(vaL - vaL/4))
//        {

//            playerPostion += 2f;
//            isMovingRight = true;
//           // Debug.Log("movval=" + tempVal);
           
//        }
//        else
//        {
//            isMovingRight = false;
//        }

      

//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            isTimer = true;

//           // tempVal = Mathf.Lerp(tempVal, -dotX / 2f, 0.1f * Time.deltaTime);
//           // Debug.Log("movVal=" + tempVal);
//        }


//        if (Input.GetKey(KeyCode.LeftShift))
//        {
//            playerPostion += 1f;
//          //  Debug.Log("movVal=" + testVal);

//        }


//        //if (Input.GetKeyDown(KeyCode.Tab))
//        //{
//        //    MoveAllSide();
//        //}


//        if (Input.GetKeyDown(KeyCode.X))
//        {
//            isMoving = false;
//        }


//        if (Input.GetKeyDown(KeyCode.R))
//        {
//            Restart();
//        }

//        if (!Input.anyKey)
//        {
//            isMovingRight = false;
//            isMovingLeft = false;
//           // moveBall();
//        }


//        // ballMov();
//        // ballMovUp();

//        // CheckHits();

//        // moveBall();

//        if (isStarted)
//        {
//            MoveAnyWhere();

//        }


//        if (Mathf.Round(curY)==240f && (Mathf.Round(curX)>= Mathf.Round(playerPostion + 5f) && Mathf.Round(curX)<= Mathf.Round(playerPostion+100f)) && !gotHit)
//        {
//           // gotHit = true;
//           // Debug.Log("gothit-1-" + gotHit + "==" + tempVal + "==curX==" + curX);
//            StartCoroutine(HitCheck("PlayerHit",-1));
          
//        }



//        if (Mathf.Round(curY) == 431f && !gotHit)
//        {
//            // gotHit = true;
//            // Debug.Log("gothit-1-" + gotHit + "==" + tempVal + "==curX==" + curX);
//            StartCoroutine(HitCheck("AiHit",1));

//        }




//        //if (Mathf.Round(curY) == 430f && Mathf.Round(curX + 5f) >= Mathf.Round(tempVal) && Mathf.Round(tempVal) <= Mathf.Round(curX + 100f) && !gotHit)
//        //{
//        //    gotHit = true;
//        //    Debug.Log("gothit-2-" + gotHit);
//        //}
//        //else
//        //{
//        //    gotHit = false;
//        //}
//    }

//    public void OnGUI()
//    {
//        if (EditorGUIUtility.currentViewWidth != windowWidth)
//        {
//            windowWidth = EditorGUIUtility.currentViewWidth;
//            maxLeftWidth =  Mathf.Round((EditorGUIUtility.currentViewWidth / 2f) - (EditorGUIUtility.currentViewWidth / 5f) / 2f);
//            //  Debug.Log("curWindowWdth-" + windowWidth);


//            dotX = vaL - 20f;
//            dotY = 435f;

//            //curX = dotX / 2f;

//           // Debug.Log(dotX);
//            hitx = Mathf.Round(Random.Range(0f, dotX));
//            hitY = Mathf.Round(Random.Range(225f, dotY));
//        }
//    }


//    public void ballMov()
//    {


//        if (curX<dotX && !isReachedMaxX)
//        {
//            // curX = Mathf.Lerp(curX, curX + 1f, 200f * Time.deltaTime);
//            isReachedMaxX = Mathf.Round(curX) == dotX ? true : false;
            
//        }else if (curX > 0f)
//        {
//            //curX = Mathf.Lerp(curX, curX - 1f, 200f * Time.deltaTime);
//            isReachedMaxX = Mathf.Round(curX) == 0 ? false : true;
//        }
//    }

//    public void ballMovUp()
//    {


//        if (curY < dotY && !isReachedMaxY)
//        {
//           // curY = Mathf.Lerp(curY, curY + 1f, 200f * Time.deltaTime);
//            isReachedMaxY = Mathf.Round(curY) == dotY ? true : false;

//        }
//        else if (curY > 160f)
//        {
//           // curY = Mathf.Lerp(curY, curY - 1f, 200f * Time.deltaTime);
//            isReachedMaxY = Mathf.Round(curY) == 160f? false : true;
//        }
//    }

//    public void MoveAllSide()
//    {

//        hitx = Mathf.Round(Random.Range(0f, dotX));
//        hitY = Mathf.Round(Random.Range(160f, dotY));

//    }

//    public void CheckHits()
//    {


//        if (curX < dotX && !isReachedMaxX)
//        {
//             curX = Mathf.Round(Mathf.Lerp(curX, curX + 1f, 200f * Time.deltaTime));
//            isReachedMaxX = Mathf.Round(curX) == dotX ? true : false;

//        }
//        else if (curX > 0f)
//        {
//            curX = Mathf.Round(Mathf.Lerp(curX, curX - 1f, 200f * Time.deltaTime));
//            isReachedMaxX = Mathf.Round(curX) == 0 ? false : true;
//        }




//        if (curY < dotY && !isReachedMaxY)
//        {
//             curY = Mathf.Round(Mathf.Lerp(curY, curY + 1f, 200f * Time.deltaTime));
//            isReachedMaxY = Mathf.Round(curY) == dotY ? true : false;

//        }
//        else if (curY > 160f)
//        {
//             curY = Mathf.Round(Mathf.Lerp(curY, curY - 1f, 200f * Time.deltaTime));
//            isReachedMaxY = Mathf.Round(curY) == 160f ? false : true;
//        }
//    }


//     public void MovPlayer1()
//     {
//        if (playerPostion < 400f && !isReached)
//        {
           
//                playerPostion += 1f;
//                oneVal = playerPostion;
//               // Debug.Log(tempVal + "=====" + oneVal);
            
           
//            isReached = Mathf.Round(playerPostion) == 400f ? true : false;
//        }

//        if (playerPostion > 0f && isReached)
//        {
//            playerPostion += -1f;
//            isReached = Mathf.Round(playerPostion) == 0f ? false : true;

//        }

//        //Debug.Log(tempVal);
//    }


//    public void moveBall()
//    {

//        //if (curX < (vaL-20) && !isReachedMaxX)
//        //{
//        //    curX = Mathf.Round(Mathf.Lerp(curX, curX + 1f, 200f * Time.deltaTime));
//        //    isReachedMaxX = Mathf.Round(curX) == (vaL - vaL / 4) ? true : false;

//        //}
//        //else if (curX > 0f)
//        //{
//        //    curX = Mathf.Round(Mathf.Lerp(curX, curX - 1f, 200f * Time.deltaTime));
//        //    isReachedMaxX = Mathf.Round(curX) == 0 ? false : true;
//        //}




//        if (curY < dotY && !isReachedMaxY)
//        {
//            curY = Mathf.Round(Mathf.Lerp(curY, curY + 1f, 200f * Time.deltaTime));
//            isReachedMaxY = Mathf.Round(curY) == dotY ? true : false;
//        }
//        else if (curY > 225f)
//        {
//            curY = Mathf.Round(Mathf.Lerp(curY, curY - 1f, 200f * Time.deltaTime));
//            isReachedMaxY = Mathf.Round(curY) == 225f ? false : true;
//        }
//    }



//    IEnumerator HitCheck(string hitInfo,int chagVal)
//    {
//        gotHit = true;
//        changeX = Random.Range(0, 1) * 2 - 1;


//        if (hitInfo == "PlayerHit")
//        {
//            Debug.Log(hitInfo + "Before==" + changeY);

//        }

//        float yVal = Mathf.Sign(changeY) * 1f;

//        changeY = yVal * -1f;
//        if (hitInfo == "PlayerHit")
//        {
//            Debug.Log(hitInfo + "After==" + changeY);
//            playerHitColor = Color.green;
//        }


//        if (hitInfo == "AiHit")
//        {
//            hitColor = Color.green;
//        }

//        yield return new WaitForSeconds(0.1f);
//        gotHit = false;
//        hitColor = Color.red;
//        playerHitColor = Color.red;

//    }


//    public void MoveAnyWhere()
//    {


//        //if (curY == 200f && !gotOut)
//        //{
//        //    gotOut = Mathf.Round(curY) == 200f ? true : false;
//        //    Debug.Log(curY);
//        //}
//        //else if (curY == 485f && !gotOut)
//        //{
//        //    gotOut = Mathf.Round(curY) == 485f ? true : false;
//        //    Debug.Log(curY);
//        //}


//        if (!gotOut)
//        {

//            //if (curY < dotY && !isReachedMaxY)
//            //{
//            //    curY = Mathf.Round(Mathf.Lerp(curY, curY + 1f , 200f * Time.deltaTime));
//            //    isReachedMaxY = Mathf.Round(curY) == dotY ? true : false;

//            //}
//            //else if (curY > 200f)
//            //{
//            //    curY = Mathf.Round(Mathf.Lerp(curY, curY - 1f, 200f * Time.deltaTime));
//            //    isReachedMaxY = Mathf.Round(curY) == 200f ? false : true;
//            //}





//            //if (curX < dotX && !isReachedMaxX)
//            //{
//            //    curX = Mathf.Round(Mathf.Lerp(curX, curX + 1f, 200f * Time.deltaTime));
//            //    isReachedMaxX = Mathf.Round(curX) == dotX ? true : false;

//            //}
//            //else if (curX > 0f)
//            //{
//            //    curX = Mathf.Round(Mathf.Lerp(curX, curX - 1f, 200f * Time.deltaTime));
//            //    isReachedMaxX = Mathf.Round(curX) == 0 ? false : true;
//            //}


//        }


//        if (isMoving)
//        {
//            //if (curY < dotY && !isReachedMaxY)
//            //{
//            //    curY = Mathf.Round(Mathf.Lerp(curY, curY + 1f, 200f * Time.deltaTime));
//            //    isReachedMaxY = Mathf.Round(curY) == dotY ? true : false;

//            //}
//            //else if (curY > 200f)
//            //{
//            //    curY = Mathf.Round(Mathf.Lerp(curY, curY - 1f, 200f * Time.deltaTime));
//            //    isReachedMaxY = Mathf.Round(curY) == 200f ? false : true;
//            //}



//            if (curY<210f)
//            {
               

//                timeText = "3";
//                dotX = vaL - 20f;
//                dotY = 485f;

//                curX = dotX / 2f;
//                curY = 430f;

//                changeY = -1f;

//                Debug.Log(dotX);
//                hitx = Mathf.Round(Random.Range(0f, dotX));
//                hitY = Mathf.Round(Random.Range(225f, dotY));
//                isStarted = false;
//                isTimer = false;
//               // isMoving = true;
//            }

//            curY = Mathf.Round(Mathf.Lerp(curY, curY + (1f*changeY), 200f * Time.deltaTime));


//            if (curX < 411f && !isReachedMaxX)
//            {
//                curX = Mathf.Round(Mathf.Lerp(curX, curX + (1f), 200f * Time.deltaTime));
//                isReachedMaxX = Mathf.Round(curX) == 411f ? true : false;
//            }
//            else if (curX > 0f)
//            {
//                curX = Mathf.Round(Mathf.Lerp(curX, curX - (1f), 200f * Time.deltaTime));
//                isReachedMaxX = Mathf.Round(curX) == 0 ? false : true;
//            }

//            // curX = Mathf.Round(Mathf.Lerp(curX, curX + (1f * changeX), 200f * Time.deltaTime));
//        }












//        if (!gotOut)
//        {
//           // curY = Mathf.Round(Mathf.Lerp(curY, curY + (1f) , 200f * Time.deltaTime));

//            //curX = Mathf.Round(Mathf.Lerp(curX, curX + 1f, 200f * Time.deltaTime));
//        }
//    }



//    public void Restart()
//    {
//        curY = 300f;
//        curX = dotX / 2f;
//        changeY = -1f;
//        isMoving = true;
//    }
//}
