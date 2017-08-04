using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    public int level_rows;
    public int level_columns;
    public int[,] tile_data;
    public TileController[,] tile_data_tilecontroller;
    string tile_level;
    int tilecontroller_index = 0;
    Transform tiles_container;

    public Sprite coin;
    public Sprite bomb;
    public Sprite diamond;
    public Sprite marked;

    public Text coin_text;
    int coins_counter;
    public Text diamond_text;
    int diamond_counter;
    public Text turns_text;
    int tiles_left;
    int tiles_left_go;

    public int win_coins;
    public int win_diamond;
    public Text win_coins_text;
    public Text win_diamond_text;
    
    public GameObject game_over;
    public GameObject game_over_extra;
    public GameObject win;
    public GameObject win_extra;

    int number_of_coins_available;
    int number_of_diamonds_available;

    int number_of_GO = 0;
    int number_of_win = 0;

    public int lives;

    void Awake()
    {
        tile_data = new int[level_rows, level_columns];
        tile_data_tilecontroller = new TileController[level_rows, level_columns];

        tiles_container = GameObject.FindGameObjectWithTag("tiles").transform;

        for (int i = 0; i < level_rows; i++)
        {
            for (int j = 0; j < level_columns; j++)
            {
                tile_data_tilecontroller[i, j] = tiles_container.GetChild(tilecontroller_index).GetComponent<TileController>();
                tilecontroller_index++;
            }
        }

        win_coins_text.text = "" + win_coins;
        win_diamond_text.text = "" + win_diamond;
        turns_text.text = "" + lives;
    }

    void Start()
    {
        generate_data();
        pop_start();
        //count_turns();
    }

    public void generate_data()
    {
        number_of_coins_available = number_of_diamonds_available = 0;
        tile_level = "";
        for (int i = 0; i < level_rows; i++)
        {
            for (int j = 0; j < level_columns; j++)
            {
                tile_data[i, j] = Random.Range(0, 4);
                tile_level = tile_level + "" + tile_data[i, j];

                tile_data_tilecontroller[i, j].generate_tile_data(tile_data[i, j], i, j);

                if (tile_data[i, j] == 2)
                    number_of_coins_available++;
                if (tile_data[i, j] == 3)
                    number_of_diamonds_available++;
            }
            tile_level = tile_level + "\n";
        }
        Debug.Log(tile_level);
    }

    void pop_start()
    {
        for (int i = 0; i < level_rows; i++)
        {
            for (int j = 0; j < level_columns; j++)
            {
                if (tile_data_tilecontroller[i, j].tile_value == 0)
                {
                    tile_data_tilecontroller[i, j].pop_when_0();
                    break;
                }
            }
        }
    }

    void restart()
    {
        coins_counter = diamond_counter = 0;
        diamond_text.text = coin_text.text = "0";
        for (int i = 0; i < level_rows; i++)
        {
            for (int j = 0; j < level_columns; j++)
            {
                tile_data_tilecontroller[i, j].restart();
            }
        }
        game_over.SetActive(false);
        game_over_extra.SetActive(false);
        win.SetActive(false);
        win_extra.SetActive(false);
    }

    public void generate_data_button()
    {
        restart();
        generate_data();
        pop_start();
        //count_turns();
    }

    public void level_(int type_of_tile)
    {
        if(type_of_tile == 2)
        {
            coins_counter++;
            number_of_coins_available--;
            coin_text.text = "" + coins_counter;
        }
        else if(type_of_tile == 3)
        {
            diamond_counter++;
            number_of_diamonds_available--;
            diamond_text.text = "" + diamond_counter;
        }
        else if (type_of_tile == 1)
        {
            coins_counter--;
            coin_text.text = "" + coins_counter;
        }

        Debug.Log(tiles_left_go);
        if (coins_counter >= win_coins && diamond_counter >= win_diamond)
            win.SetActive(true);
        else if (tiles_left_go+1 == level_rows * level_columns)
        {
            lives--;
            turns_text.text = "" + lives;
            if(lives == 0)
                game_over_extra.SetActive(true);
            else
                game_over.SetActive(true);
        }

        //turns_counter--;
        //turns_text.text = "" + turns_counter;
    }

    void count_tiles_left_win()
    {
        tiles_left = 0;

        for (int i = 0; i < level_rows; i++)
        {
            for (int j = 0; j < level_columns; j++)
            {
                if (tile_data_tilecontroller[i, j].is_active)
                    tiles_left++;
            }
        }
    }

    public void count_tiles_left()
    {
        tiles_left_go = 0;

        for (int i = 0; i < level_rows; i++)
        {
            for (int j = 0; j < level_columns; j++)
            {
                if (!tile_data_tilecontroller[i, j].is_active)
                    tiles_left_go++;
            }
        }

    }

}
