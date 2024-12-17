using UnityEngine;
using UnityEngine.UI; // Potrzebne do używania UI

public class PlayerColorUI : MonoBehaviour
{
    public SpriteRenderer playerSprite; // Referencja do SpriteRenderera gracza
    public Image colorDisplayImage;     // Referencja do UI Image, który pokaże kolor
    public Text colorValueText;         // Opcjonalnie: referencja do UI Text, żeby pokazać wartości RGB

    void Update()
    {
        if (playerSprite != null && colorDisplayImage != null)
        {
            // Pobierz aktualny kolor gracza
            Color currentColor = playerSprite.color;

            // Przypisz kolor do UI Image
            colorDisplayImage.color = currentColor;

            // Opcjonalnie: Wyświetl wartości RGB w UI Text
            if (colorValueText != null)
            {
                colorValueText.text = $"R: {Mathf.RoundToInt(currentColor.r * 255)}, " +
                                      $"G: {Mathf.RoundToInt(currentColor.g * 255)}, " +
                                      $"B: {Mathf.RoundToInt(currentColor.b * 255)}";
            }
        }
    }
}