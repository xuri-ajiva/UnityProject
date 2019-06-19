using System.Data;
using System.Threading;
using UnityEngine;

public class WeaponControler : MonoBehaviour {
    public float dammage = 10;
    public float range   = 100;
    public float speed   = 40;
    public float timeBetweanSchoot;

    public Camera Camara;

    public GameObject projektile;

    private bool Hit_Scan = true;

    private float current_hit;

    // Start is called before the first frame update
    private void Start() { }


    // Update is called once per frame
    private void Update() {
        this.current_hit += 1000 * Time.deltaTime;

        if ( this.projektile != null )
            if ( Input.GetButton( "Fire1" ) ) {
                if ( this.Hit_Scan ) {
                    Schoot_Hit();
                }
                else {
                    Schoot_project();
                }
            }
    }

    private void Schoot_project() { }


    public void Schoot_Hit() {
        if ( this.current_hit <= this.timeBetweanSchoot ) return;
        RaycastHit hit;

        if ( Physics.Raycast( this.Camara.transform.position, this.Camara.transform.forward, out hit,
            this.range ) ) {
            GameObject pro = Instantiate( this.projektile ) as GameObject;
            pro.transform.position = hit.point;
            pro.transform.LookAt( this.Camara.transform );
            pro.transform.parent = this.transform.parent;

            Destroy( pro, 4 );

            //Debug.Log(hit.transform.name);
            Thaget taget = hit.transform.GetComponent<Thaget>();
            if ( taget != null ) {
                taget.TagDamage( this.dammage );
            }
        }

        this.current_hit = 0;
    }
}