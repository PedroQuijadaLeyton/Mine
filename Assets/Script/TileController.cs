using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{
    
    int row_number = 0;
    int column_number = 0;
    int number_of_1;
    int number_of_2;
    int number_of_3;
    public int tile_value;
    LevelController lc;
    public bool is_active = true;
    bool is_being_hold_down = false;
    float timer_hold_down = 0;
    float timer_hold_down_total = 0;
    bool is_marked = false;

    // Use this for initialization
    void Awake ()
    {
        lc = FindObjectOfType<LevelController>();
    }

    public void generate_tile_data(int tile_value_, int row_number_, int column_number_)
    {
        //asign value to the tile
        // tile_value = lc.tile_data[row_number, column_number];
        row_number = row_number_;
        column_number = column_number_;
        tile_value = tile_value_;
        //transform.GetChild(0).GetComponent<Text>().text = "" + tile_value_;

        number_of_1 = 0;
        number_of_2 = 0;
        number_of_3 = 0;

        //read the neighbours
        for (int i = row_number - 1; i <= row_number + 1; i++)
        {
            for (int j = column_number - 1; j <= column_number + 1; j++)
            {
                if (i >= 0 && i < lc.tile_data.GetLength(0) && j >= 0 && j < lc.tile_data.GetLength(0) && !(row_number == i && column_number == j))
                    if (lc.tile_data[i, j] == 1)
                        number_of_1++;
                    else if(lc.tile_data[i, j] == 2)
                        number_of_2++;
            }
        }
    }

    void on_click()
    {
        if(is_active)
        {
            if (tile_value != 0)
            {

                //asign value to the tile
                tile_value = lc.tile_data[row_number, column_number];

                //traps
                number_of_1 = 0;
                //coins
                number_of_2 = 0;
                //diamon
                number_of_3 = 0;

                //read the neighbours
                for (int i = row_number - 1; i <= row_number + 1; i++)
                {
                    for (int j = column_number - 1; j <= column_number + 1; j++)
                    {
                        if (i >= 0 && i < lc.tile_data.GetLength(0) && j >= 0 && j < lc.tile_data.GetLength(0) && !(row_number == i && column_number == j))
                            if (lc.tile_data[i, j] == 1)
                                number_of_1++;
                            else if (lc.tile_data[i, j] == 2)
                                number_of_2++;
                            else if (lc.tile_data[i, j] == 3)
                                number_of_3++;
                    }
                }
                //asign the text/data for the player
                transform.GetChild(1).GetComponent<Text>().text = "" + number_of_1;
                transform.GetChild(2).GetComponent<Text>().text = "" + number_of_2;
                transform.GetChild(3).GetComponent<Text>().text = "" + number_of_3;
            }
            else
            {
                pop_when_0();
            }
        }
        
        
    }

    public void pop_when_0()
    {
        hide_tile();
        //asign value to the tile
        tile_value = lc.tile_data[row_number, column_number];

        //traps
        number_of_1 = 0;
        //coins
        number_of_2 = 0;
        //diamons
        number_of_3 = 0;

        //read the neighbours
        for (int i = row_number - 1; i <= row_number + 1; i++)
        {
            for (int j = column_number - 1; j <= column_number + 1; j++)
            {
                if (i >= 0 && i < lc.tile_data.GetLength(0) && j >= 0 && j < lc.tile_data.GetLength(0) && !(row_number == i && column_number == j))
                    if (lc.tile_data[i, j] == 1)
                        number_of_1++;
                    else if (lc.tile_data[i, j] == 2)
                        number_of_2++;
                    else if (lc.tile_data[i, j] == 3)
                        number_of_3++;
            }
        }
        //asign the text/data for the player
        transform.GetChild(1).GetComponent<Text>().text = "" + number_of_1;
        transform.GetChild(2).GetComponent<Text>().text = "" + number_of_2;
        transform.GetChild(3).GetComponent<Text>().text = "" + number_of_3;

        for (int i = row_number - 1; i <= row_number + 1; i++)
        {
            for (int j = column_number - 1; j <= column_number + 1; j++)
            {
                if (i >= 0 && i < lc.tile_data.GetLength(0) && j >= 0 && j < lc.tile_data.GetLength(0) && lc.tile_data_tilecontroller[i, j].is_active)
                {
                    if (i == row_number || j == column_number)
                        lc.tile_data_tilecontroller[i, j].on_click();

                }

            }
        }
    }

    void hide_tile()
    {
        is_active = false;
        gameObject.SetActive(false);
    }

    public void restart()
    {
        is_active = true;
        gameObject.SetActive(true);
        transform.GetChild(1).GetComponent<Text>().text = "";
        transform.GetChild(2).GetComponent<Text>().text = "";
        transform.GetChild(3).GetComponent<Text>().text = "";
        GetComponent<Image>().sprite = null;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        is_being_hold_down = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        if (!is_marked && timer_hold_down_total < 1.5f)
        {
            if (tile_value == 1)
                GetComponent<Image>().sprite = lc.bomb;
            else if (tile_value == 2)
                GetComponent<Image>().sprite = lc.coin;
            else if (tile_value == 3)
                GetComponent<Image>().sprite = lc.diamond;

            if (is_active)
            {
                lc.level_(tile_value);
                on_click();
                is_active = false;
            }
        }

        timer_hold_down_total = 0;
        is_being_hold_down = false;
        timer_hold_down = 0;
        lc.count_tiles_left();
    }

    void Update()
    {
        if (is_being_hold_down)
        {
            timer_hold_down += Time.deltaTime;
            timer_hold_down_total += Time.deltaTime;
        }

        if (timer_hold_down > 2.0f)
        {
            if(!is_marked)
            {
                is_marked = true;
                GetComponent<Image>().sprite = lc.marked;
            }
            else
            {
                is_marked = false;
                GetComponent<Image>().sprite = null;
            }
            timer_hold_down = 0;
        }
    }
}
