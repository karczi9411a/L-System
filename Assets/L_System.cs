using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class L_System : MonoBehaviour
{
	[SerializeField]
	public Transform prefab_obiekt;
	[SerializeField]
	private GameObject obiekt;

	[SerializeField]
	private InputField aksjomat_IF;
	[SerializeField]
	private InputField iterajca_IF;
	[SerializeField]
	private InputField skala_IF;
	[SerializeField]
	private InputField kat_IF;
	[SerializeField]
	private InputField regula1_IF;
	[SerializeField]
	private InputField regula2_IF;
	[SerializeField]
	private InputField regula3_IF;
	[SerializeField]
	private InputField regula4_IF;
	[SerializeField]
	private InputField regula5_IF;

	//A->AB,B->A = AB,ABA,ABAAB
	[SerializeField]
	private string aksjomat;
	[SerializeField]
	private string regula_1;
	[SerializeField]
	private string regula_2;
	[SerializeField]
	private string regula_3;
	[SerializeField]
	private string regula_4;
	[SerializeField]
	private string regula_5;

	private Dictionary<char, string> zasady = new Dictionary<char, string>();

	[SerializeField]
	private int iteracja;
	[SerializeField]
	private string obecny_string;
	[SerializeField]
	private float krok;
	[SerializeField]
	private float kat;

	private Stack<Transform_informacje> Stos_transfromacji = new Stack<Transform_informacje>();

	private bool dalej_generowac = false;

	public void Przyklad_1()
	{
		iteracja = 500;
		aksjomat = "XF";
		regula_1 = "F+[[X)]-X]-F[(-FX]+)X";
		regula_2 = "FF";

		krok = 0.4f;
		kat = 30;

		Wypisz();
	}

	public void Przyklad_2()
	{
		iteracja = 75;
		aksjomat = "F";
		regula_1 = "FF+[+F)F-F]-[(F+F)F]";  
		krok = 0.7f;
		kat = 25;

		Wypisz();
	}

	public void Przyklad_3()
	{
		iteracja = 100;
		aksjomat = "F--F--F";
		regula_1 = "F + F--F + F";  
		krok = 0.4f;
		kat = 60;

		Wypisz();
	}



	public void Przycisk_Generuj()
	{
		Zaakceptuj_dane();
	 	Wypisz();

		obecny_string = aksjomat;

		if (aksjomat.Length <= 0 || regula_1.Length <=0)
		{
			Debug.Log("brak");
		}
		else zasady.Add(aksjomat[0],regula_1);

		if (aksjomat.Length > 1 & aksjomat.Length < 3)//2 regula
		{
			zasady.Add(aksjomat[1], regula_2);
		}
		else if (aksjomat.Length > 2 & aksjomat.Length < 4)//3
		{
			zasady.Add(aksjomat[2], regula_3);
		}
		else if (aksjomat.Length > 3 & aksjomat.Length < 5)//4
		{
			zasady.Add(aksjomat[3], regula_4);
		}
		else if (aksjomat.Length > 4 & aksjomat.Length < 6)//5
		{
			zasady.Add(aksjomat[4], regula_5);
		}
		/*
		else if(aksjomat.Length > 5)
		{
			Debug.Log("Za duzo");
		}
		*/
		StartCoroutine(WygenerujLS()); //zaczynam odliczanie
	}

	void Zaakceptuj_dane()
	{
		aksjomat = aksjomat_IF.text;
		regula_1 = regula1_IF.text;
		regula_2 = regula2_IF.text;
		regula_3 = regula3_IF.text;
		regula_4 = regula4_IF.text;
		regula_5 = regula5_IF.text;

		int.TryParse(iterajca_IF.text, out iteracja);
		float.TryParse(skala_IF.text, out krok);
		float.TryParse(kat_IF.text, out kat);
	}

	void Wypisz()
	{
		aksjomat_IF.text = aksjomat.ToString();
		regula1_IF.text = regula_1;
		regula2_IF.text = regula_2;
		regula3_IF.text = regula_3;
		regula4_IF.text = regula_4;
		regula5_IF.text = regula_5;

		iterajca_IF.text = iteracja.ToString();
		skala_IF.text = krok.ToString();
		kat_IF.text = kat.ToString();
	}

	IEnumerator WygenerujLS() //ograniczenie generowania i robienie przerw aby byl widoczny progress
	{
		for(int i=0;i<iteracja;i++) //ograniczenie iteracji
		{
			//Debug.Log("iteracja "+i);
			if (dalej_generowac !=true)
			{
				dalej_generowac = true;
				StartCoroutine(Wygeneruj());
				//break;
			}
			else
			{
				yield return new WaitForSeconds(0.01f);
			}
		}
	}

	IEnumerator Wygeneruj() //glowna preocedura, 
	{
		string nowy_string = "";
		char[] string_Znak = obecny_string.ToCharArray(); //nasz obecny string czyli aksjomat, bedzie sie dodawal tworzac lancuch

		Debug.Log("Dlugosc " + string_Znak.Length);

		for (int i = 0; i < string_Znak.Length; i++)
		{
			char obecny_znak = string_Znak[i];

			if (zasady.ContainsKey(obecny_znak)) //czy istnieje klucz(obcnyznak) w naszym dictionary(tablica znak,string)
			{
				nowy_string += zasady[obecny_znak]; //jesli tak, dodanie do lancuszka
			}
			else
			{
				nowy_string += obecny_znak.ToString(); //dodaje do obecnego, dzieki temu dlugosc lancucha sie zwiekszy 
			}
		}

		obecny_string = nowy_string; 
		Debug.Log(obecny_string);
		string_Znak = obecny_string.ToCharArray();

		for (int i = 0; i < string_Znak.Length; i++) //cala magia koordynacji
		{
			char obecny_znak = string_Znak[i];

			if (obecny_znak == 'F')//porusz się do przodu, rysujac
			{
				Vector3 Ini_pozycja = transform.position;
				transform.Translate(Vector3.forward * krok);

				TworzObiekt(prefab_obiekt, Ini_pozycja, transform.position);
				
				yield return null; //czekam, symulacja animacji 
			}
			if (obecny_znak == 'f')//porusz się do przodu
			{
				Vector3 Ini_pozycja = transform.position;
				transform.Translate(Vector3.forward * krok);
				yield return null;
			}
			else if (obecny_znak == '+')//rotacja o kat +
			{
				transform.Rotate(Vector3.up * kat);
			}
			else if (obecny_znak == '-')//rotacja o kat -
			{
				transform.Rotate(Vector3.up * -kat);
			}
			else if (obecny_znak == '[')//kladz na stos
			{
				Transform_informacje t_in = new Transform_informacje();
				t_in.pozycja = transform.position;
				t_in.rotacja = transform.rotation;
				Stos_transfromacji.Push(t_in);
			}
			else if (obecny_znak == ']')//zdjemuj ze stosu
			{
				Transform_informacje t_in = Stos_transfromacji.Pop();
				transform.position = t_in.pozycja;
				transform.rotation = t_in.rotacja;
			}
			else if (obecny_znak == ')')//odbroc sie wokol wlasnej osi o okreslony kat w prawo
			{
				transform.Rotate(Vector3.right * kat);
			}
			else if (obecny_znak == '(')//odbroc sie wokol wlasnej osi o okreslony kat w lewo
			{
				transform.Rotate(Vector3.right * -kat);
			}
			/*
			else if (obecny_znak == '^')//pochyl sie do przodu o okreslony kat
			{

			}
			else if (obecny_znak == '&')//pochyl sie do tyłu o okreslony kat
			{

			}
			*/
		}
		dalej_generowac = false;
	}

	private void TworzObiekt(Transform prefab_obiekt, Vector3 poczatek, Vector3 koniec) //klonuje nasz obiekt, czyli cylinder
	{
		obiekt = Instantiate<GameObject>(prefab_obiekt.gameObject, Vector3.zero, Quaternion.identity);
		//obiekt = Instantiate(prefab_obiekt.gameObject, Vector3.zero, Quaternion.identity, transform);
		Aktualizuj_pozycje(obiekt, poczatek, koniec);
	}

	private void Aktualizuj_pozycje(GameObject obiekt, Vector3 poczatek, Vector3 koniec) // uatulnia pozycje obeiktu 3d z dwoma wspolrzednymi
	{
		Vector3 offset = koniec - poczatek;
		Vector3 position = poczatek + (offset / 2.0f);

		obiekt.transform.position = position;
		obiekt.transform.LookAt(poczatek);
		Vector3 localScale = obiekt.transform.localScale;
		localScale.z = (koniec - poczatek).magnitude;
		obiekt.transform.localScale = localScale;
	}

	public void Czysc()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
	}

}
