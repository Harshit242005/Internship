import React from 'react'
import styles from './Styles/Landing.module.css';
import { Link } from 'react-router-dom';
function Landing() {
    return (
        <>
            <div className={styles.Container}>
                <div className={styles.LeftContainer}>
                    <p className={styles.heading1}>Writeup</p>
                    <div className={styles.buttons}>
                        <Link className={styles.link} to="/Signup"><button>Signup</button></Link>
                        <Link className={styles.link} to="/Login"><button>Login</button></Link>
                    </div>
                </div>
                <div className={styles.RightContainer}>
                    <div className={styles.About}>
                        <p className={styles.heading2}><span>w</span>riteup</p>
                        <p className={styles.text1}>An easy application to start blogging and engaging with friends and making new ones</p>
                    </div>
                </div>
            </div>
        </>
    )
}

export default Landing