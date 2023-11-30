
import styles from './layout.module.css'

export default function Mylayout() {
    return (
        <div style={{ Height: '300px', width: '500px', display: 'flex', background: 'green' }}>
            {/* <svg version="1.1"
                    baseProfile="full"
                    width="300" height="200"
                    xmlns="http://www.w3.org/2000/svg">
                    <rect width="100%" height="100%" stroke="red" stroke-width="4" fill="yellow" />
                    <circle cx="150" cy="100" r="80" fill="green" />
                    <text x="150" y="115" font-size="16" text-anchor="middle" fill="white">RUNOOB SVG TEST</text>
                </svg> */}
            <svg xmlns="http://www.w3.org/2000/svg" version="1.1">
                <rect textDecoration='12' x='0' width="50" height="50" style={{ fill: 'rgb(0,0,255)', strokeWidth: '1', stroke: 'rgb(0,0,0)' }} />
                <text x="0" y="15" fill="red">I love SVG</text>
                <line x1="50" y1="25" x2="200" y2="75" style={{ stroke: 'rgb(255,0,0)', strokeWidth:'3'}} />
                <rect x='200' y='50' width="50" height="50" style={{ fill: 'rgb(255,0,255)', strokeWidth: '1', stroke: 'rgb(0,0,0)' }} />
            </svg>
            <button>123456</button>
        </div>
    )
}