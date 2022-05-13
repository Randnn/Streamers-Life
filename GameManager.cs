using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool StartedGame = false;
        
    public bool paused = false;

    public TMP_Text followers_Text;
    public TMP_Text subscribers_Text;
    public TMP_Text money_Text;
    public TMP_Text time_Text;
    public TMP_Text day_Text;
    public TMP_Text snackNumber_Text;
    public TMP_Text pizzaNumber_Text;
    public TMP_Text saladNumber_Text;
    public TMP_Text burgerNumber_Text;
    public TMP_Text sodaNumber_Text;
    public TMP_Text waterNumber_Text;
    public TMP_Text juiceNumber_Text;
    public TMP_Text energyDrink_Text;

    public Image progressBar;

    public int subscribers_Number = 0;
    public int followers_Number = 0;
    public int money_Number = 0;
    public int hour = 0;
    public int minute = 0;
    public int day_Number = 1;
    public int health = 100;
    public int food = 100;
    public int water = 100;
    public int snackAmount = 0;
    public int pizzaAmount = 0;
    public int burgerAmount = 0;
    public int saladAmount = 0;
    public int sodaAmount = 0;
    public int waterAmount = 0;
    public int juiceAmount = 0;
    public int energyDrinkAmount = 0;
    
    public Slider healthBar;
    public Slider hungerBar;
    public Slider thirstBar;
    public Slider fridgeHealthBar;
    public Slider fridgeHungerBar;
    public Slider fridgeThirstBar;
    
    public float progress = 0f;

    public Canvas currentCanvas;
    public Canvas mainUI;
    public Canvas shopUI;
    public Canvas theFridgeUI;
    public Canvas pauseUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if (StartedGame == false)
        {   
            StartedGame = true;

            currentCanvas = mainUI;

            StartCoroutine(DayNightCycle());
            StartCoroutine(HungerBehavior());
            StartCoroutine(ThirstBehavior());
            
            followers_Text.text = "Followers: " + followers_Number.ToString();
            subscribers_Text.text = "Subscribers: " + subscribers_Number.ToString();
            money_Text.text = "Money: " + money_Number.ToString();
            progressBar.fillAmount = progress;
            time_Text.text = hour.ToString() + " " + minute.ToString();
            day_Text.text = "Day: " + day_Number.ToString();

            PlayerData data = SaveSystem.LoadPlayer(SaveGameManager.saveGameNumber);

            string path = Application.persistentDataPath + "/savedata" + SaveGameManager.saveGameNumber.ToString() + ".fun"; ;

            if (File.Exists(path))
            {
                subscribers_Number = data.subscribers;
                followers_Number = data.followers;
                money_Number = data.money;
                progress = data.progressBar;
                hour = data.hour;
                minute = data.minute;
                day_Number = data.day_Number;
                health = data.health;
                food = data.food;
                water = data.water;
                snackAmount = data.snackAmount;
                pizzaAmount = data.pizzaAmount;
                burgerAmount = data.burgerAmount;
                saladAmount = data.saladAmount;
                sodaAmount = data.sodaAmount;
                waterAmount = data.waterAmount;
                juiceAmount = data.juiceAmount;
                energyDrinkAmount = data.energyDrinkAmount;
                Debug.Log("Data Loaded from " + Application.persistentDataPath + "/savedata" + SaveGameManager.saveGameNumber.ToString() + ".fun");
            }


            progressBar.fillAmount = progress;
            followers_Text.text = "Followers: " + followers_Number.ToString();
            subscribers_Text.text = "Subscribers: " + subscribers_Number.ToString();
            money_Text.text = "Money: " + money_Number.ToString();
            progressBar.fillAmount = progress;
            time_Text.text = hour.ToString() + " : " + minute.ToString();
            day_Text.text = "Day: " + day_Number.ToString();

              
        }
    }

    public void Pause()
    {
        if (paused == false)
        {
            paused = true;
            UpdateUI();
            currentCanvas.gameObject.SetActive(false);
            pauseUI.gameObject.SetActive(true);
        }
        else if (paused == true)
        {
            paused = false;
            UpdateUI();
            pauseUI.gameObject.SetActive(false);
            currentCanvas.gameObject.SetActive(true);
        }
    }

    public void Stream()
    {
            ProgressUpdate(0.1f);
    }

    void ProgressUpdate(float a)
    {
        int b;
        progress = progress + a;
        progressBar.fillAmount = progress;
        b = Random.Range(1, 100);

        if (b < 6)
        {
            Donation(Random.Range(1, 15));
        }

        if (progress >= 1.0f)
        {
            progress = 0f;
            progressBar.fillAmount = progress;
            UpdateStreamInfo();
        }
    }

    public void SaveAndQuit()
    {
        SaveSystem.SaveGame(this, SaveGameManager.saveGameNumber);
        SceneManager.LoadScene(0);
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(this, SaveGameManager.saveGameNumber);
    }

    private void UpdateStreamInfo()
    {
        followers_Number = followers_Number + Random.Range(5, 50);
        subscribers_Number = subscribers_Number + Random.Range(0, 10);
        UpdateUI();
    }

    void UpdateUI()
    {
        followers_Text.text = "Followers: " + followers_Number.ToString();
        subscribers_Text.text = "Subscribers: " + subscribers_Number.ToString();
        money_Text.text = "Money: " + money_Number.ToString();
        snackNumber_Text.text = "Snacks: " + snackAmount;
        pizzaNumber_Text.text = "Pizzas: " + pizzaAmount;
        saladNumber_Text.text = "Salads: " + saladAmount;
        burgerNumber_Text.text = "Burgers: " + burgerAmount;
        sodaNumber_Text.text = "Sodas: " + sodaAmount;
        waterNumber_Text.text = "Waters: " + waterAmount;
        juiceNumber_Text.text = "Juices: " + juiceAmount;
        energyDrink_Text.text = "Energy Drinks: " + energyDrinkAmount;
    }

    void Donation(int amount)
    {
        money_Number = money_Number + amount;
    }

    IEnumerator DayNightCycle()
    {
        if (!paused)
        {
            minute = minute + 1;

            if (minute >= 60)
            {
                minute = 0;
                hour = hour + 1;

                if (hour >= 24)
                {
                    hour = 0;
                    day_Number = day_Number + 1;
                    day_Text.text = "Day: " + day_Number;
                }
            }

            if (hour < 10 && minute < 10)
            {
                time_Text.text = "0" + hour.ToString() + " : 0" + minute.ToString();
            }
            else if (hour >= 10 && minute < 10)
            {
                time_Text.text = hour.ToString() + " : 0" + minute.ToString();
            }
            else if (hour <= 10 && minute >= 10)
            {
                time_Text.text = "0" + hour.ToString() + " : " + minute.ToString();
            }
            else if (hour >= 10 && minute >= 10)
            {
                time_Text.text = hour.ToString() + " : " + minute.ToString();
            }
        }
        

        yield return new WaitForSeconds(1f);

        StartCoroutine(DayNightCycle());
    }

    IEnumerator HealthBehavior()
    {
        health = health - 1;

        healthBar.value = health;

        yield return new WaitForSeconds(1f);

        if (food == 0 || water == 0)
        {
            StartCoroutine(HealthBehavior());
        }
        else
        {
            StopCoroutine(HealthBehavior());
        }
    }

    IEnumerator HungerBehavior()
    {
        if (!paused)
        {
            food = food - 1;

            hungerBar.value = food;

            if (food <= 0)
            {
                food = 0;
                StartCoroutine(HealthBehavior());
            }
        }
        

        yield return new WaitForSeconds(4f);

        StartCoroutine(HungerBehavior());
    }

    IEnumerator ThirstBehavior()
    {
        if (!paused)
        {
            water = water - 1;

            thirstBar.value = water;

            if (water <= 0)
            {
                water = 0;
                StartCoroutine(HealthBehavior());
            }
        }
        

        yield return new WaitForSeconds(6f);

        StartCoroutine(ThirstBehavior());
    }
    
    public void BuySnack()
    {
        if (money_Number >= 10)
        {
            money_Number = money_Number - 10;
            snackAmount = snackAmount + 1;
        }
        
    }

    public void BuyPizza()
    {
        if (money_Number >= 16)
        {
            money_Number = money_Number - 16;
            pizzaAmount = pizzaAmount + 1;
        }
    }

    public void BuyBurger()
    {
        if (money_Number >= 20)
        {
            money_Number = money_Number - 20;
            burgerAmount = burgerAmount + 1;
        }
    }

    public void BuySalad()
    {
        if (money_Number >= 18)
        {
            money_Number = money_Number - 18;
            saladAmount = saladAmount + 1;
        }
    }

    public void BuySoda()
    {
        if (money_Number >= 10)
        {
            money_Number = money_Number - 10;
            sodaAmount = sodaAmount + 1;
        }
    }

    public void BuyWater()
    {
        if (money_Number >= 20)
        {
            money_Number = money_Number - 20;
            waterAmount = waterAmount + 1;
        }
    }

    public void BuyJuice()
    {
        if (money_Number >= 6)
        {
            money_Number = money_Number - 6;
            juiceAmount = juiceAmount + 1;
        }
    }

    public void BuyEnergyDrink()
    {
        if (money_Number >= 30)
        {
            money_Number = money_Number - 30;
            energyDrinkAmount = energyDrinkAmount + 1;
        }
    }

    public void ShopBackButton()
    {
        shopUI.gameObject.SetActive(false);
        mainUI.gameObject.SetActive(true);
        currentCanvas = mainUI;
        healthBar.value = health;
        hungerBar.value = food;
        thirstBar.value = water;
    }
    
    public void ShopButton()
    {
        mainUI.gameObject.SetActive(false);
        shopUI.gameObject.SetActive(true);
        currentCanvas = shopUI;
    }

    public void EatSnack()
    {
        if (snackAmount > 0)
        {
            snackAmount = snackAmount - 1;
            food = food + 3;
            fridgeHungerBar.value = food;
            UpdateUI();
        }

    }

    public void EatPizza()
    {
        if (pizzaAmount > 0)
        {
            pizzaAmount = pizzaAmount - 1;
            food = food + 8;
            fridgeHungerBar.value = food;
            UpdateUI();
        }
    }

    public void EatBurger()
    {
        if (burgerAmount > 0)
        {
            burgerAmount = burgerAmount - 1;
            food = food + 15;
            fridgeHungerBar.value = food;
            UpdateUI();
        }
    }

    public void EatSalad()
    {
        if (saladAmount > 0)
        {
            saladAmount = saladAmount - 1;
            food = food + 10;
            fridgeHungerBar.value = food;
            UpdateUI();
        }
    }

    public void EatSoda()
    {
        if (sodaAmount > 0)
        {
            sodaAmount = sodaAmount - 1;
            water = water + 5;
            fridgeThirstBar.value = water;
            UpdateUI();
        }
    }

    public void EatWater()
    {
        if (waterAmount > 0)
        {
            waterAmount = waterAmount - 1;
            water = water + 12;
            fridgeThirstBar.value = water;
            UpdateUI();
        }
    }

    public void EatJuice()
    {
        if (juiceAmount > 0)
        {
            juiceAmount = juiceAmount - 1;
            water = water + 2;
            fridgeThirstBar.value = water;
            UpdateUI();
        }
    }

    public void EatEnergyDrink()
    {
        if (energyDrinkAmount > 0)
        {
            energyDrinkAmount = energyDrinkAmount - 1;
            water = water + 20;
            fridgeThirstBar.value = water;
            UpdateUI();
        }
    }

    public void TheFridgeButton()
    {
        mainUI.gameObject.SetActive(false);
        theFridgeUI.gameObject.SetActive(true);
        currentCanvas = theFridgeUI;
        fridgeHealthBar.value = health;
        fridgeHungerBar.value = food;
        fridgeThirstBar.value = water;
        UpdateUI();
    }

    public void FridgeBackButton()
    {
        theFridgeUI.gameObject.SetActive(false);
        mainUI.gameObject.SetActive(true);
        currentCanvas = mainUI;
        healthBar.value = health;
        hungerBar.value = food;
        thirstBar.value = water;
    }
}
